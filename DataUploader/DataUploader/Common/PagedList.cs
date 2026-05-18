namespace DataUploader
{
    public class PagedList<TEntity>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页集合
        /// </summary>
        public List<TEntity> Items { get; set; }

    }
}
