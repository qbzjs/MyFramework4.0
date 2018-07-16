namespace MyFramework
{
    /// <summary>
    /// 版本管理UI组件
    /// </summary>
    public interface IVersionManageViewComp
    {
        void UpdataView(string TitleStr, string DescribeStr, float Progress);
        void UpdataView(string TitleStr, string DescribeStr, float Progress01, float Progress02);
    }

    public class VersionManageViewCompBase : Model_BaseViewComp<VersionManagerModel>, IVersionManageViewComp
    {
        public override void Load(ModelContorlBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        public virtual void UpdataView(string TitleStr, string DescribeStr, float Progress)
        {

        }

        public virtual void UpdataView(string TitleStr, string DescribeStr, float Progress01, float Progress02)
        {

        }
    }
}
