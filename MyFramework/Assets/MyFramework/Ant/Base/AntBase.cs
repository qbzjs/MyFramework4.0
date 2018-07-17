/// <summary>
/// 蚁巢框架 蚂蚁
/// </summary>
namespace MyFramework {

    public interface IAntBase
    {
        void OnCreate(params object[] _Agr);
        void OnDestroy();
    }

    public interface IUpdataAnt
    {
        void Update(float time);
    }

    public interface IFixedUpdateAnt
    {
        void FixedUpdate();
    }

    public abstract class AntBase : IAntBase
    {
        public virtual void OnCreate(params object[] _Agr)
        {

        }

        public virtual void OnDestroy()
        {

        }
    }
}
