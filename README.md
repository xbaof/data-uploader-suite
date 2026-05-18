# DataUploader

基于 .NET 8 Windows Forms 的数据上传器应用程序，提供数据上传日志管理、定时任务调度和许可证验证功能。

## 项目结构

```
├── DataUploader/                    # 主应用程序
│   ├── DataUploader/               # 应用代码
│   │   ├── Application/            # 应用层
│   │   │   ├── Authorization/      # 授权验证
│   │   │   ├── Serilog/            # 日志配置
│   │   │   └── Utils/              # 工具类
│   │   ├── Common/                 # 通用配置
│   │   ├── Configs/                # 配置文件
│   │   ├── Models/                 # 数据模型
│   │   ├── Tasks/                  # 定时任务
│   │   │   └── Jobs/               # 任务实现
│   │   └── DataUploader.csproj     # 项目文件
│   └── DataUploader.sln            # 解决方案
└── LicenseGenerator/               # 许可证生成工具
```

## 技术栈

| 组件 | 版本 | 用途 |
|-----|------|-----|
| .NET | 8.0 | 运行时框架 |
| AntdUI | 2.0.15 | Windows Forms UI组件库 |
| Quartz | 3.15.1 | 定时任务调度 |
| SqlSugar | 5.1.4 | ORM框架（SQLite） |
| Serilog | 7.0.0 | 结构化日志 |
| NPOI.Mapper | 6.2.2 | Excel导出 |
| Costura.Fody | 6.0.0 | 单文件打包 |

## 功能特性

- **日志管理**：分页查询、时间范围筛选、关键词搜索
- **数据导出**：支持Excel导出（限制60天内数据）
- **任务调度**：基于Quartz的定时任务执行
- **授权管理**：许可证验证和更新
- **主题切换**：支持明暗主题
- **托盘图标**：最小化到系统托盘运行
- **单实例运行**：保证只有一个实例运行

## 快速开始

### 环境要求

- .NET 8.0 SDK
- Visual Studio 2022 (推荐)

### 构建与运行

```bash
# 克隆项目
git clone <repository-url>
cd DataUploader

# 构建项目
dotnet build DataUploader/DataUploader.csproj -c Release

# 运行项目
dotnet run --project DataUploader/DataUploader.csproj
```

### 配置说明

配置文件位于 `Configs/config.json`：

```json
{
  "ThirdDB": "数据库连接字符串",
  "BaseUrl": "API基础地址",
  "PKI": "PKI配置",
  "Tasks": [
    {
      "Name": "任务名称",
      "Cron": "Cron表达式",
      "AssemblyName": "程序集名称",
      "ClassName": "任务类全名",
      "Enable": true,
      "Params": "任务参数JSON"
    }
  ]
}
```

## 定时任务

系统内置三个定时任务：

| 任务名称 | Cron表达式 | 执行频率 |
|---------|-----------|---------|
| 数据同步任务 | `0 0/2 * * * ?` | 每2分钟 |
| 示例任务 | `0 0/2 * * * ?` | 每2分钟 |
| 日志清理任务 | `0 0 2 * * ?` | 每天凌晨2点 |

## 许可证验证

系统采用RSA加密进行许可证验证：

1. 获取加密的授权数据
2. 使用RSA私钥解密
3. 验证过期时间
4. 验证授权码和硬件指纹
5. 验证通过后启动任务调度器

## 使用LicenseGenerator生成许可证

打开 `LicenseGenerator/LicenseGenerator.sln` 解决方案，运行许可证生成工具：

1. 输入授权信息
2. 生成加密的许可证数据
3. 在DataUploader中导入许可证

## 项目维护

### 日志位置

日志文件位于 `bin/Debug/net8.0-windows/Logs/`：
- `InfoLog/` - 应用日志
- `UploadLog/` - 上传日志

### 数据库

SQLite数据库位于 `DataBase/DataUploader.db`

## 许可证

MIT License
