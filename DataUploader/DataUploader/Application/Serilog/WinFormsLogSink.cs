using AntdUI;
using DataUploader.Application.Utils;
using Serilog.Core;
using Serilog.Events;
using static NPOI.HSSF.Util.HSSFColor;

namespace DataUploader.Application.Serilog
{
    public class WinFormsLogSink : ILogEventSink
    {
        private readonly Input _logInput;
        private readonly string _newLine = "\n";
        private readonly object _lockObject = new object();

        public WinFormsLogSink(Input logInput)
        {
            _logInput = logInput;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage();
            var strDate = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            var level = logEvent.Level;

            // 根据日志级别选择颜色
            Color? levelColor = null;
            if (level == LogEventLevel.Warning)
            {
                levelColor = Color.DarkOrange;
            }
            else if (level >= LogEventLevel.Error)
            {
                levelColor = Color.Red;
            }
            // 构建日志行
            var logLine = $"[{strDate}] {message}{_newLine}";

            _logInput.Invoke((MethodInvoker)(() =>
            {
                lock (_lockObject)
                {
                    // 检查是否需要处理MaxLength限制
                    HandleMaxLengthLimit(logLine.Length);

                    int currentPosition = _logInput.Text.Length;
                    // 添加日志行到输入控件
                    _logInput.AppendText(logLine);

                    if (levelColor != null)
                    {
                        // 为日志内容设置颜色
                        _logInput.SetStyle(currentPosition, logLine.Length, fore: levelColor);
                    }

                    // 滚动到最后一行
                    _logInput.ScrollToCaret();
                }
            }));
        }

        /// <summary>
        /// 处理Input控件的MaxLength限制，确保不会超出最大长度
        /// 根据换行符截取，保证都是完整的行数据
        /// </summary>
        /// <param name="newTextLength">新增文本的长度</param>
        private void HandleMaxLengthLimit(int newTextLength)
        {
            if (_logInput.MaxLength <= 0)
                return;

            int currentLength = _logInput.Text.Length;
            int totalLength = currentLength + newTextLength;

            if (totalLength <= _logInput.MaxLength)
                return;

            // 计算需要移除的字符数
            int excessLength = totalLength - _logInput.MaxLength;

            // 为了防止频繁截取，我们移除比超出部分更多的内容（移除前25%的内容）
            int removeLength = Math.Max(excessLength, _logInput.MaxLength / 4);

            // 确保移除长度不超过当前文本长度
            removeLength = Math.Min(removeLength, currentLength);

            if (removeLength > 0)
            {
                // 获取当前文本
                string currentText = _logInput.Text;

                // 找到移除位置后的第一个换行符，确保移除完整的行
                int nextNewLineIndex = currentText.IndexOf(_newLine, removeLength);

                // 如果找到了换行符，则移除到该换行符位置（包含换行符）
                if (nextNewLineIndex >= 0)
                {
                    // 移除到换行符后的位置
                    removeLength = nextNewLineIndex + _newLine.Length;
                }
                else
                {
                    // 如果没有找到换行符，说明剩余内容不足一行，移除所有内容
                    removeLength = currentLength;
                }

                // 执行移除操作
                if (removeLength < currentLength)
                {
                    string newText = currentText[removeLength..];
                    _logInput.Text = newText;
                }
                else
                {
                    // 如果移除长度大于等于当前长度，清空文本
                    _logInput.Text = string.Empty;
                }
            }
        }
    }
}