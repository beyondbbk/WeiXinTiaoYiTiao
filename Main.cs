using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoJump.Robot.Simple;

namespace ShowAndroidModel
{
    public partial class Main : Form
    {
        private string _adbPath = string.Empty;
        private string _driverPath = string.Empty;
        private double rate = Convert.ToDouble(ConfigurationManager.AppSettings["rate"]);//倍率默认是1.1

        public Main()
        {
            InitializeComponent();
        }

        enum MouseState
        {
            None = 0,
            MouseLeftDown = 1,
            MouseRightDown = 2,
        }

        private MouseState _MouseState = MouseState.None;
        /// <summary>
        /// 是否停止刷新界面
        /// </summary>
        private bool isStop = false;
        /// <summary>
        /// 是否在画画
        /// </summary>
        private bool isDraw = false;

        private bool isAuto = true;//默认开启自动模式

        /// <summary>
        /// 是否存在安卓
        /// </summary>
        private bool HasAndroid = false;

        /// <summary>
        /// 设备后插入延时执行
        /// </summary>
        private System.Timers.Timer myTimer = new System.Timers.Timer(1200);
        private void Form1_Load(object sender, EventArgs e)
        {
            //检测Adb工具
            string temp = Directory.GetParent(Environment.CurrentDirectory).FullName;
            _adbPath = $"{Directory.GetParent(temp).FullName}\\AdbTool\\adb.exe";

            if (!File.Exists(_adbPath))
            {
                MessageBox.Show("adb.exe文件已丢失，无法启动！", "严重错误");
                Environment.Exit(0);
            }

            //检测adb驱动，不是必须
            _driverPath = $"{Directory.GetParent(temp).FullName}\\AdbTool\\ADBDriverInstaller.exe";
            if (!File.Exists(_adbPath))
            {
                AddLogAsync("ADB相关驱动检测丢失！");
            }

            //检测temp文件夹
            if (!Directory.Exists(Environment.CurrentDirectory + "\\temp"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\temp");
            }
            Environment.CurrentDirectory = Environment.CurrentDirectory + "\\temp";

            MessageBox.Show($"使用说明:"+Environment.NewLine+
                "1，确保手机已进入开发者模式并开启了USB调试功能；"+ Environment.NewLine +
                "2，已进入微信跳一跳游戏并开始游戏；" + Environment.NewLine +
                "3，将手机通过USB连接到电脑。"+ Environment.NewLine + Environment.NewLine +
                "以上完成后再点击“确定”按钮，程序将自动匹配落点并控制小人起跳。"+ Environment.NewLine + Environment.NewLine +
                "\t\t\t\tBy 勇敢的心bbk 2018.01.20", "提示");

            //完善提示
            var toolTip1 = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 500,
                ShowAlways = true
            };
            toolTip1.SetToolTip(this.btnUp, "当小人跳跃距离太小时，请增大倍率");
            toolTip1.SetToolTip(this.btnDown, "当小人跳跃距离太大时，请减小倍率");
            this.labelShow.Text = "当前倍率：" + rate;
            //检测手机
            CheckHasAndroidModel();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x219)
            {
                Debug.WriteLine("WParam：{0} ,LParam:{1},Msg：{2}，Result：{3}", m.WParam, m.LParam, m.Msg, m.Result);
                if (m.WParam.ToInt32() == 7)
                {
                    AddLogAsync("检测到设备更改...");
                    CheckHasAndroidModel();
                    myTimer.Start();
                }
            }
            try
            {
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        /// <summary>
        /// 检测是否存在手机
        /// </summary>
        private void CheckHasAndroidModel()
        {
            var text = cmdAdb("shell getprop ro.product.model", false);//获取手机型号
            if (text.Contains("no devices") || string.IsNullOrWhiteSpace(text))
            {
                HasAndroid = false;
                isStop = true;

                AddLogAsync("没有检查到安卓手机，返回：" + text);
                var mboxResult = MessageBox.Show($"无法检测到手机，请确保手机已经通过USB连接到电脑。{Environment.NewLine}如已连接，可能是PC未安装驱动，是否进行驱动检测？", "错误", MessageBoxButtons.YesNo);
                if (mboxResult == DialogResult.Yes)
                {
                    Process.Start(_driverPath);
                    Environment.Exit(0);
                }
            }
            else
            {
                AddLogAsync("已连接到安卓手机：" + text.Trim());
                HasAndroid = true;
                isStop = false;

                RefreshPicAsync();//开始获取截屏
            }
        }

        /// <summary>
        /// 执行adb命令
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="ischeck"></param>
        /// <returns></returns>
        private string cmdAdb(string arguments, bool ischeck = true)
        {
            if (ischeck && !HasAndroid)
            {
                return string.Empty;
            }
            var ret = string.Empty;
            using (var p = new Process())
            {
                p.StartInfo.FileName = _adbPath;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;   //重定向标准输入   
                p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出   
                p.StartInfo.RedirectStandardError = true;   //重定向错误输出   
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                ret = p.StandardOutput.ReadToEnd();
                p.Close();
            }
            return ret;
        }


        /// <summary>
        /// 消息提示日志
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task AddLogAsync(string msg)
        {
            await Task.Run(() =>
            {
                this.Invoke(new Action(() =>
                {
                    MsgList.Items.Add(msg);
                    MsgList.SelectedIndex = MsgList.Items.Count - 1;
                }));
            });
        }

        /// <summary>
        /// 刷新Pic控件图像
        /// </summary>
        private async void RefreshPicAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (isStop)
                    {
                        return;
                    }
                    //截屏并获取图片
                    var fileTempName = "temp.png";
                    cmdAdb("shell screencap -p /sdcard/" + fileTempName);
                    cmdAdb("pull /sdcard/" + fileTempName);
                    if (string.IsNullOrEmpty(fileTempName)) continue;
                    if (File.Exists(Environment.CurrentDirectory+"\\"+fileTempName))
                    {
                        using (var temp = Image.FromFile(fileTempName))
                        {
                            this.Invoke(new Action(() =>
                            {
                                pictureBox1.Image = new Bitmap(temp);
                            }));
                        }
                        TryAutoExcute(fileTempName);//尝试自动执行
                        GC.Collect();
                        if (File.Exists(fileTempName))
                        {
                            try
                            {
                                File.Delete(fileTempName);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 黑人底部位置
        /// </summary>
        Point startPoint;
        /// <summary>
        /// 图案中心或者白点位置
        /// </summary>
        Point endPoint;

        /// <summary>
        /// 人工辅助操作模块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            var me = ((System.Windows.Forms.MouseEventArgs)(e));
            if (me.Button == MouseButtons.Left)//按下左键是黑人底部的坐标
            {
                isAuto = false;
                startPoint = ((System.Windows.Forms.MouseEventArgs)(e)).Location;
                AddLogAsync("已人工纠正小人位置为：" + startPoint.X + "," + startPoint.Y);
            }
            else if (me.Button == MouseButtons.Right)//按下右键键是黑人底部的坐标
            {
                endPoint = ((System.Windows.Forms.MouseEventArgs)(e)).Location;
                AddLogAsync("已人工纠正落点位置为：" + endPoint.X + "," + endPoint.Y);
                isAuto = true;
            }
        }

        //向手机发送触摸指令
        private void ExcuteSwipe(int time = 0)
        {
            if (time ==0 )
            {
                //计算两点直接的距离
                var value = Math.Sqrt(Math.Abs(startPoint.X - endPoint.X) * Math.Abs(startPoint.X - endPoint.X) + Math.Abs(startPoint.Y - endPoint.Y) * Math.Abs(startPoint.Y - endPoint.Y));
                time = (int)(4.7 * value);
            }
            AddLogAsync($"准备起跳，模拟触摸：{time}毫秒");
            cmdAdb($"shell input swipe 100 100 200 200 {(time*rate).ToString("0")}");
            Thread.Sleep(2000);//延时2秒，太快会截图不准确
        }

        /// <summary>
        /// 获取起始点和终止点
        /// </summary>
        /// <param name="pngPath"></param>
        /// <returns></returns>
        private bool TryAutoExcute(string pngPath)
        {
            if (!isAuto) return false;

            using (var bmp = new Bitmap(pngPath))
            {
                try
                {
                    AddLogAsync("尝试解析小人起点和目标落点...");
                    var result = new SimpleRobot().GetNextTap(bmp);
                    AddLogAsync("解析成功...");
                    ExcuteSwipe(result.Duration);
                    this.Invoke(new Action(() =>
                    {
                        pictureBox1.Image = new Bitmap(bmp);
                    }));
                    return true;
                }
                catch (Exception ex)
                {
                    AddLogAsync("自动捕获出现异常：" + ex.ToString());
                    AddLogAsync("请人工辅助操作。");
                    AddLogAsync("左键单击小人底部，再右键单击目标落点即可。");
                    isAuto = false;
                    return false;
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            rate = rate+0.01;
            Config.UpdateConnectionStringsConfig(rate.ToString());
            this.labelShow.Text = "当前倍率："+rate.ToString();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            rate = rate - 0.01;
            Config.UpdateConnectionStringsConfig(rate.ToString());
            this.labelShow.Text = "当前倍率：" + rate.ToString();
        }

    }
}
