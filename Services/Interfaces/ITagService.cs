namespace TestApp.Services.Interfaces
{
    public interface ITagService
    {

        int Add(string tagName);

        int GetTagIdByName(string tagName);

        void Delete(string tagName);

        bool IsTagAlreadyExit(string tag);


    }
}
