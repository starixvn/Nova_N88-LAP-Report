namespace Fanex.Athena.Report.Common.Services.Interfaces
{
    public interface IDataPermissionService
    {
        void ClearDataByPermission<T>(T data);
    }
}