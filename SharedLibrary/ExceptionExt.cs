
using System.Text;

namespace SharedLibrary
{
    public static class ExceptionExt
    {
        public static string MessageWithInners(this Exception ex)
        {
            Exception current = ex;
            StringBuilder sb = new();
            int counter = 0;
            sb.AppendLine(ex.Message);
            while(current.InnerException is not null)
            {
                current = current.InnerException;
                sb.AppendLine($"Inner {++counter}: {current.Message}");
            } 
            return sb.ToString();
        }
    }
}
