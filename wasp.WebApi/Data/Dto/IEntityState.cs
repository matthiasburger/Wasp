namespace wasp.WebApi.Data.Dto
{
    public enum DtpRecordState
    {
        Error = -1,
        None = 0,
        Created = 1,
        Read = 2,
        Updated = 3,
        Removed = 4
    }
    
    public interface IEntityState<out T>
    {
        T Entity { get; }
        
        DtpRecordState State { get; }
    }
}