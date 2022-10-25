using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace JSFW.PREZI
{

    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }

    internal static class Ux
    {
        /// <summary>
        /// 컨트롤 비동기 호출! 
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="action"></param>
        public static void DoAsync<TControl>(this TControl ctrl, Action<TControl> action) where TControl : Control
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(action, ctrl);
            }
            else
            {
                action(ctrl);
            }
        }

        /// <summary>
        /// Object To XML
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="value">object Instance</param>
        /// <returns></returns>
        public static string Serialize<T>(this T value)
        {
            if (value == null) return string.Empty;
            string xml = "";
            try
            {
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (var stringWriter = new System.IO.StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                    {
                        xmlSerializer.Serialize(xmlWriter, value);
                        xml = stringWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // 변환 중 Error!
                System.Diagnostics.Debugger.Log(0, typeof(T).GetType().Name + " Serialize", ex.Message);
            }
            return xml;
        }

        /// <summary>
        /// Xml String !
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(this string xml) where T : class, new()
        {
            T obj = default(T);
            try
            {
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (var stringReader = new System.IO.StringReader(xml))
                {
                    using (var reader = XmlReader.Create(stringReader, new XmlReaderSettings()))
                    {
                        obj = xmlSerializer.Deserialize(reader) as T;
                    }
                }
            }
            catch (Exception ex)
            {
                // 변환 중 Error!
                System.Diagnostics.Debugger.Log(0, typeof(T).GetType().Name + " Serialize", ex.Message);
            }
            return obj;
        }
        #region 컨트롤 캡쳐

        public static int ThumbnailWith = 160;
        public static int ThumbnailHeight = 90;

        /// <summary>
        /// The WM_PRINT drawing options
        /// </summary>
        [Flags]
        enum DrawingOptions
        {
            /// <summary>
            /// Draws the window only if it is visible.
            /// </summary>
            PRF_CHECKVISIBLE = 1,

            /// <summary>
            /// Draws the nonclient area of the window.
            /// </summary>
            PRF_NONCLIENT = 2,
            /// <summary>

            /// Draws the client area of the window.
            /// </summary>
            PRF_CLIENT = 4,

            /// <summary>
            /// Erases the background before drawing the window.
            /// </summary>
            PRF_ERASEBKGND = 8,

            /// <summary>
            /// Draws all visible children windows.
            /// </summary>
            PRF_CHILDREN = 16,

            /// <summary>
            /// Draws all owned windows.
            /// </summary>
            PRF_OWNED = 32
        }


        /// <summary>
        /// 스크롤 생긴 컨트롤을 지정해야 그안에 컨트롤이 가득 찍혀짐. 
        /// </summary>
        /// <param name="ctrl"></param>
        public static System.Drawing.Image ControlShot(this Control ctrl, int offsetWH, bool copyClipboard = true)
        {
            //http://stackoverflow.com/questions/1881317/c-sharp-windows-form-control-to-image
            //todo : 컨트롤을 스크린을 찍어서 클립보드에 넣어준다.
            if (ctrl is ScrollableControl)
            {
                // 스크롤을 0으로 바꿔주어야 스크린 찍을때 안짤림. 
                ((ScrollableControl)ctrl).HorizontalScroll.Value = 0;
                ((ScrollableControl)ctrl).VerticalScroll.Value = 0;
                ((ScrollableControl)ctrl).AutoScrollPosition = new System.Drawing.Point(0, 0);
            }

            const int WM_PRINT = 791;

            using (System.Drawing.Bitmap screenshot = new System.Drawing.Bitmap(
                                                            ctrl.Width + offsetWH,
                                                            ctrl.Height + offsetWH))
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(screenshot))
            {
                try
                {
                    SendMessage(ctrl.Handle,
                                  WM_PRINT,
                                  g.GetHdc().ToInt32(),
                                  (int)(DrawingOptions.PRF_CHILDREN |
                                            DrawingOptions.PRF_CLIENT |
                                            DrawingOptions.PRF_NONCLIENT |
                                            DrawingOptions.PRF_OWNED));
                }
                finally
                {
                    g.ReleaseHdc();
                }
                //screenshot.Save("temp.bmp");  
                if(copyClipboard)
                    Clipboard.SetImage(screenshot);
                return screenshot.GetThumbnailImage(ThumbnailWith, ThumbnailHeight, delegate { return false; }, IntPtr.Zero);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        #endregion
        public static DateTime? ToDateTime(this object obj, string Fmt)
        {
            DateTime _datetime;
            //try
            //{ 
            //    _datetime = DateTime.ParseExact("" + obj, "yyyy-MM-dd tt H:mm:ss", null, System.Globalization.DateTimeStyles.AssumeLocal);
            //    return _datetime;
            //}
            //catch (Exception)
            //{ } 

            if (string.IsNullOrEmpty(Fmt)) Fmt = "yyyy-MM-dd tt hh:mm:ss";

            //if (obj.IsNull() || string.IsNullOrEmpty(("" + obj))) return null;
            //else
            //{
            //    return DateTime.ParseExact("" + obj, Fmt, null, System.Globalization.DateTimeStyles.AssumeLocal);
            //}
            string dt = ("" + obj).Trim();

            if (string.IsNullOrEmpty(dt)) return null;

            if (dt.Contains("오전") || dt.Contains("오후"))
            {
                if (dt.Length == "yyyy-MM-dd 오후 hh:mm:ss".Length)
                {
                    if (DateTime.TryParseExact(dt, "yyyy-MM-dd tt hh:mm:ss", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                    {
                        return _datetime;
                    }
                }
                else if (dt.Length == "yyyy-MM-dd 오후 H:mm:ss".Length)
                {
                    if (DateTime.TryParseExact(dt, "yyyy-MM-dd tt H:mm:ss", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                    {
                        return _datetime;
                    }
                }
            }
            else
            {
                if (dt.Contains("-"))
                {
                    if (dt.Length == "yyyy-MM-dd hh:mm:ss".Length)
                    {
                        if (DateTime.TryParseExact(dt, "yyyy-MM-dd hh:mm:ss", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                        {
                            return _datetime;
                        }
                    }
                    else if (dt.Length == "yyyy-MM-dd h:mm:ss".Length)
                    {
                        if (DateTime.TryParseExact(dt, "yyyy-MM-dd h:mm:ss", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                        {
                            return _datetime;
                        }
                    }
                    else if (dt.Length == "yyyy-MM-dd hh:mm:ss.fff".Length)
                    {
                        if (DateTime.TryParseExact(dt, "yyyy-MM-dd hh:mm:ss.fff", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                        {
                            return _datetime;
                        }
                    }
                    else if (dt.Length == "yyyy-MM-dd h:mm:ss.fff".Length)
                    {
                        if (DateTime.TryParseExact(dt, "yyyy-MM-dd h:mm:ss.fff", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                        {
                            return _datetime;
                        }
                    }
                    else if (dt.Length == "yyyy-MM-dd".Length)
                    {
                        if (DateTime.TryParseExact(dt, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                        {
                            return _datetime;
                        }
                    }
                }

                dt = dt.Replace("-", "");
                if (dt.Length == "yyyyMMdd".Length)
                {
                    if (DateTime.TryParseExact(dt, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                    {
                        return _datetime;
                    }
                }
                else if (dt.Length == "yyyyMM".Length)
                {
                    if (DateTime.TryParseExact(dt + "01", "yyyyMMdd", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                    {
                        return _datetime;
                    }
                }
                else if (dt.Length == "yyyy".Length)
                {
                    if (DateTime.TryParseExact(dt + "0101", "yyyyMMdd", null, System.Globalization.DateTimeStyles.AssumeLocal, out _datetime))
                    {
                        return _datetime;
                    }
                }
            }
            return null;
        }

        public static string Toyyyy_MM_dd(this DateTime datetime, string Fmt)
        {
            if (string.IsNullOrEmpty(Fmt)) Fmt = "yyyy-MM-dd";
            return datetime.ToString(Fmt);
        }

        public static T To<T>(this object obj, object DefaultValue) where T : IConvertible
        {
            if (typeof(T).BaseType == typeof(Enum))
            {
                if (Enum.IsDefined(typeof(T), obj))
                {
                    return (T)Enum.Parse(typeof(T), "" + obj);
                }
                else
                {
                    return (T)DefaultValue;
                }
            }

            TypeCode typecode = (TypeCode)Enum.Parse(typeof(TypeCode), typeof(T).Name);

            if (string.IsNullOrEmpty("" + obj))
            {
                switch (typecode)
                {
                    case TypeCode.Boolean:
                        break;
                    case TypeCode.Byte:
                        break;
                    case TypeCode.Char:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.SByte:
                        break;
                    case TypeCode.String:
                        break;

                    case TypeCode.Double:
                    case TypeCode.Decimal:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        obj = DefaultValue ?? "0";
                        break;
                    default:
                        obj = "";
                        break;
                    case TypeCode.DateTime:
                        return (T)obj;
                }
            }
            return (T)Convert.ChangeType(obj, typecode);
        }

        // 디렉토리  
        public static bool ExistsAndCreate(string DirectoryPath)
        {
            try
            {
                if (Directory.Exists(DirectoryPath))
                {
                    return true;
                }
                string tempFullPath = Path.GetFullPath(DirectoryPath).Replace(@"\\", @"\");
                string RootPath = Path.GetPathRoot(tempFullPath);
                string[] Split = tempFullPath.Split('\\');
                string PassDirectory = "";
                foreach (var item in Split)
                {
                    PassDirectory += item + @"\";
                    if (item == RootPath) continue;

                    if (Directory.Exists(PassDirectory)) continue;

                    Directory.CreateDirectory(PassDirectory);
                }

                return Directory.Exists(DirectoryPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DialogResult Alert(this object msg)
        {
            return MessageBox.Show(string.Format("{0}", msg));
        }
        public static DialogResult AlertWarning(this string msg)
        {
            return MessageBox.Show(string.Format("{0}", msg, "경고"));
        }

        public static void DebugWarning(this string msg)
        {
            string title = "";
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
                title = sf.GetMethod().Name + ":";
            }
            catch (Exception)
            {
            }

            System.Diagnostics.Debug.WriteLine(msg, title);
        }
        public static void DebugWarning(this string msg, string title)
        {
            System.Diagnostics.Debug.WriteLine(msg, title);
        }
        /// <summary>
        /// Yes or No [ Question ]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult Confirm(this object msg)
        {
            return MessageBox.Show(string.Format("{0}", msg), "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 화면을 뷰모드중이다. 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        internal static bool IsViewMode(this Control ctrl)
        {
            bool IsViewMode = false;

            if (ctrl != null)
            {
                IDesignView iview = ctrl.FindForm() as IDesignView;
                if (iview != null)
                {
                    IsViewMode = iview.DesignView == DesignView.View;
                }
            }
            return IsViewMode;
        }

        /// <summary>
        /// 화면을 디자인 중이다.
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        internal static bool IsDesignMode(this Control ctrl)
        {
            if (ctrl == null) return false;
            return !ctrl.IsViewMode();
        }
    }

    /// <summary>
    /// 디자인 뷰/ 모드
    /// </summary>
    public enum DesignView
    {
        Design,
        View
    }

    public interface IDesignView
    {
        /// <summary>
        /// 디자인 변경 여부 > SaveButton 색상 또는 닫을때, 다른 화면으로 이동할때 체크대상이 됨.
        /// </summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// 디자인모드 / 뷰모드
        /// </summary>
        DesignView DesignView { get; } 
    }



}
