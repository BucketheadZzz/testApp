using Antlr.Runtime.Tree;

namespace TestApp.Services.Interfaces
{
    public interface ITagService
    {

        int AddTag(string tagName);

        int GetTagIdByName(string tagName);
        void RemoveTag(string tagName);

        bool IsTagAlreadyExit(string tag);

        void RemoveMappingByTagId(int tagId);

    }
}
