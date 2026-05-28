# 随机抽奖系统（Chapter21_1）

基于 **WPF** 与 **MVVM** 的桌面抽奖程序：预生成 100 个随机手机号，点击开始后快速滚动显示（中间四位脱敏），点击停止后定格为当前幸运号码。

![alt text](assets/ui-screenshot.png)

## 功能特性

- **随机号码池**：启动时生成 100 个不重复的中国大陆手机号（常见号段）
- **滚动抽奖**：后台线程每 50ms 随机切换显示号码，营造抽奖动感
- **隐私脱敏**：展示格式为 `前3位 + **** + 后4位`（如 `138****5678`）
- **开始 / 停止**：绿色「开始抽奖」、红色「停止抽奖」，防止重复启动
- **状态提示**：界面实时显示「抽取中」「停止」等状态

## 技术栈

| 项目 | 说明 |
|------|------|
| 框架 | .NET 10（`net10.0-windows`） |
| UI | WPF（XAML + 数据绑定） |
| 架构 | MVVM（`RandomLottery` + `RelayCommand`） |
| 并发 | 后台 `Thread` + `Dispatcher` 更新 UI |

## 项目结构

```
Chapter21_1/
├── Chapter21_1/                 # 主程序
│   ├── MainWindow.xaml          # 主界面
│   ├── MainViewModel/
│   │   └── RandomLottery.cs     # 视图模型与命令
│   └── Tools/
│       └── PhoneNumberGenerator.cs  # 手机号生成
├── Chapter21_1.slnx             # 解决方案
├── .vs/                         # Visual Studio 配置
└── README.md
```

## 环境要求

- Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/download)（或与本项目 `TargetFramework` 一致的 SDK）

## 快速开始

在仓库根目录执行：

```powershell
cd Chapter21_1
dotnet run
```

或在 Visual Studio / Rider 中打开 `Chapter21_1` 项目并运行。

## 使用说明

1. 启动程序后，主界面显示标题、状态区与幸运号码区。
2. 点击 **开始抽奖**：号码开始快速滚动，状态为「抽取中」。
3. 点击 **停止抽奖**：滚动结束，当前号码即为本次结果；状态为「停止」。
4. 可再次点击开始，进行下一轮抽取。

## 核心实现说明

- **号码生成**：`PhoneNumberGenerator.Generate(100)` 使用 `HashSet` 保证 100 个号码不重复。
- **滚动逻辑**：`GetNum()` 在后台线程循环随机索引，经 `Application.Current.Dispatcher.Invoke` 更新 `PhoneNumberDisplay`。
- **线程安全**：`_shouldStop` 使用 `volatile`；`Join(1000)` 等待线程结束；`IsBackground = true` 避免进程无法退出。

## 界面设计

界面采用紫罗兰渐变标题、卡片式布局与圆角按钮，配色与布局定义于 `MainWindow.xaml`。

## 许可证

本项目为 C# 学习示例代码，可按学习需要自由使用与修改。
