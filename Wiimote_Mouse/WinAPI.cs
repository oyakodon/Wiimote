using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Wiimote_Mouse
{
    public class WinAPI
    {
        /// <summary>
        /// 指定された文字列と一致するクラス名とウィンドウ名を持つウィンドウのハンドルを返す
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 指定されたウィンドウのタイトルバーのテキストをバッファに返す
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);

        /// <summary>
        /// 指定されたウィンドウの設定を変更
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(
            IntPtr hWnd,               // ウインドウのハンドル
            int hWndInsertAfter,    // 配置順序のハンドル
            int X,                  // 横方向の位置
            int Y,                  // 縦方向の位置
            int cx,                 // 幅
            int cy,                 // 高さ
            uint uFlags              // ウインドウ位置のオプション
        );

        /// <summary>
        /// ウィンドウを移動する
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int MoveWindow(
            IntPtr hWnd,
            int x,
            int y,
            int nWidth,
            int nHeight,
            int bRepaint
            );

        /// <summary>
        /// 指定されたウィンドウをフォアグラウンドにし、アクティブにする
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(
            IntPtr hWnd
            );

        /// <summary>
        /// 指定された座標を含むウィンドウのハンドルを取得する
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr WindowFromPoint(Point lpPoint);

        /// <summary>
        /// カーソルのある座標を取得する
        /// </summary>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        /// <summary>
        /// 指定されたウィンドウの左上端と右下端の座標をスクリーン座標で取得する
        /// </summary>
        [DllImport("User32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd,
            ref RECT lpRect);

        /// <summary>
        /// フォアグラウンドのウィンドウのハンドルを取得する
        /// </summary>
        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// マウスの移動やマウスボタンのクリックを合成します。
        /// </summary>
        /// <param name="dwFlags">移動とクリックのオプション</param>
        /// <param name="dx">水平位置または移動量</param>
        /// <param name="dy">垂直位置または移動量</param>
        /// <param name="cButtons">ホイールの移動</param>
        /// <param name="dwExtraInfo">アプリケーション定義の情報</param>
        /// <remarks></remarks>
        [DllImport("USER32.DLL")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //dx と dy の各パラメータは、正規化された絶対座標を意味します。
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        //マウスが移動したことを示します。
        public const int MOUSEEVENTF_MOVE = 0x1;
        //左ボタンが押されたことを示します。
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        //左ボタンが離されたことを示します。
        public const int MOUSEEVENTF_LEFTUP = 0x4;
        //右ボタンが押されたことを示します。
        public const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        //右ボタンが離されたことを示します。
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        //中央ボタンが離されたことを示します。
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        //中央ボタンが離されたことを示します。
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;

        //Windows NT/2000：マウスにホイールが実装されている場合、そのホイールが回転したことを示します。移動量は、dwData パラメータで指定します。
        public const int MOUSEEVENTF_WHEEL = 0x800;
        //Windows 2000：X ボタンが押されたことを示します。
        public const int MOUSEEVENTF_XDOWN = 0x80;
        //Windows 2000：X ボタンが離されたことを示します。
        public const int MOUSEEVENTF_XUP = 0x100;

        // ホイールの移動量
        public const int WHEEL_DELTA = 120;

        /// <summary>
        /// Point構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// RECT構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;

    }
}
