namespace Fanex.Athena.Report.Common.Model.MetaModels
{
    public interface IPlayerListMetaModel
    {
        int DataLimitBy { get; }

        int IsDataLimitDesc { get; }

        int DataLimitValue { get; }
    }
}