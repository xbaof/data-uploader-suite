using SqlSugar;
using System.Data;
using System.Linq.Expressions;
using DbType = SqlSugar.DbType;

namespace DataUploader.Application
{
    public class SqlSugarOperation : IDisposable
    {
        private SqlSugarClient _db;
        private bool _disposed = false;

        public SqlSugarClient Db => _db;

        public SqlSugarOperation(string connectionString, DbType dbType = DbType.SqlServer)
        {
            _db = GetInstance(connectionString, dbType);
        }

        public static SqlSugarClient GetInstance(string connectionString, DbType dbType)
        {
            try
            {
                SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = dbType,
                    IsAutoCloseConnection = true
                });
                return db;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("数据库连接初始化失败", ex);
            }
        }

        public async Task<DataTable> GetDataTable(string strSql)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SqlSugarOperation));

            var result = await _db.Ado.GetDataTableAsync(strSql);
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db?.Dispose();
                    _db = null;
                }
                _disposed = true;
            }
        }

        ~SqlSugarOperation()
        {
            Dispose(false);
        }

        #region 查询
        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<T> GetById<T>(object objId, bool blnUseCache = false) where T : class, new()
        {
            return await _db.Queryable<T>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        }

        public List<T> GetList<T>() where T : class, new()
        {
            return _db.Queryable<T>().ToList();
        }

        public async Task<List<T>> GetListAsync<T>() where T : class, new()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> expression, string orderByStr = null) where T : class, new()
        {
            return await _db.Queryable<T>().Where(expression).OrderByIF(!string.IsNullOrEmpty(orderByStr), orderByStr).ToListAsync();
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> expression, string orderByStr = null) where T : class, new()
        {
            return _db.Queryable<T>().Where(expression).OrderByIF(!string.IsNullOrEmpty(orderByStr), orderByStr).ToList();
        }
        /// <summary>
        /// 根据实体名称动态查询数据
        ///  //var list = db.Queryable<dynamic>().AS("order ").Where("id=@id", new { id = 1 }).ToList();//没实体一样
        /// </summary>
        /// <param name="tableName">表明</param>
        /// <param name="whereString">查询条件</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        public List<dynamic> GetListDataByDynamic(string tableName, string whereString, object parameters = null)
        {
            return _db.Queryable<dynamic>().AS(tableName).Where(whereString, parameters).ToList();
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, string strOrderByFileds = null) where TEntity : class, new()
        {
            RefAsync<int> total = 0;
            var items = await _db.Queryable<TEntity>().Where(expression).OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).ToPageListAsync(pageIndex, pageSize, total);
            return CreatePagedList(items, total, pageIndex, pageSize);
        }

        public PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, string strOrderByFileds = null) where TEntity : class, new()
        {
            var total = 0;
            var items = _db.Queryable<TEntity>().Where(expression).OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds).ToPageList(pageIndex, pageSize, ref total);
            return CreatePagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 创建 <see cref="PagedList{TEntity}"/> 对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="items">分页内容的对象集合</param>
        /// <param name="total">总条数</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <returns></returns>
        private PagedList<TEntity> CreatePagedList<TEntity>(List<TEntity> items, int total, int pageIndex, int pageSize)
        {
            return new PagedList<TEntity>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Items = items,
                Total = total,
            };
        }

        #endregion

        public int Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return _db.Deleteable<T>().Where(expression).ExecuteCommand();
        }
        public async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return await _db.Deleteable<T>().Where(expression).ExecuteCommandAsync();
        }

        #region 事务
        public void BeginTran()
        {
            _db.Ado.BeginTran();
        }

        public void CommitTran()
        {
            _db.Ado.CommitTran();
        }

        public void RollbackTran()
        {
            _db.Ado.RollbackTran();
        }

        public void EndCommitTran()
        {
            try
            {
                _db.Ado.CommitTran();
            }
            catch (Exception)
            {
                _db.Ado.RollbackTran();
                throw; // 重新抛出异常以保持堆栈跟踪
            }
        }

        #endregion

        #region 插入
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="listData">数据list集合</param>
        /// <returns>插入的记录数</returns>
        public int InsertList<T>(List<T> listData) where T : class, new()
        {
            if (!listData.Any())
                return 0;
            return _db.Insertable(listData).ExecuteCommand();
        }

        public async Task<int> InsertListAsync<T>(List<T> listData) where T : class, new()
        {
            if (!listData.Any())
                return 0;
            return await _db.Insertable(listData).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量插入数据(适用于500条以下数据)
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="listData">插入数据集合</param>
        /// <returns>插入的记录数</returns>
        public int InsertListOptimized<T>(List<T> listData) where T : class, new()
        {
            if (!listData.Any())
                return 0;
            return _db.Insertable(listData).UseParameter().ExecuteCommand();
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns>插入的实体</returns>
        public T Insert<T>(T entity) where T : class, new()
        {
            //直接返回新增的实体
            return _db.Insertable(entity).ExecuteReturnEntity();
        }
        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns>插入的实体</returns>
        public async Task<T> InsertSync<T>(T entity) where T : class, new()
        {
            //直接返回新增的实体
            return await _db.Insertable(entity).ExecuteReturnEntityAsync();
        }

        public async Task<bool> InsertOrUpdate<T>(T data) where T : class, new()
        {
            return await _db.Storageable(data).ToStorage().AsInsertable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 不存在才插入
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Insert<T>(Dictionary<string, object> dictionary, string tableName) where T : class, new()
        {
            return await _db.Storageable(dictionary, tableName).ToStorage().AsInsertable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 插入数据，返回自增列id
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>主键</returns>
        public int InsertEntityReturnIdentity<T>(T entity) where T : class, new()
        {
            return _db.Insertable<T>(entity).ExecuteReturnIdentity();
        }
        #endregion

        public async Task<int> Update<T>(List<T> entities) where T : class, new()
        {
            return await _db.Updateable(entities).ExecuteCommandAsync();
        }

        public async Task<int> Update<T>(T entity, Expression<Func<T, object>> columnsToUpdate) where T : class, new()
        {
            return await _db.Updateable(entity).UpdateColumns(columnsToUpdate).ExecuteCommandAsync();
        }

        public async Task<int> Update<T>(Expression<Func<T, bool>> whereExpression,
                                         Expression<Func<T, T>> updateExpression) where T : class, new()
        {
            return await _db.Updateable<T>()
                         .Where(whereExpression)
                         .SetColumns(updateExpression)
                         .ExecuteCommandAsync();
        }
    }
}