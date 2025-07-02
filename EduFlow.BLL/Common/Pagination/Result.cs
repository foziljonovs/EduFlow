namespace EduFlow.BLL.Common.Pagination;

public class Result<T>
{
    public bool Status { get; }
    public string Message { get; }
    public List<T> Data { get; }

    public Result(bool status, string mesage, List<T> data)
    {
        this.Status = status;
        this.Message = mesage;
        this.Data = data;
    }
}
