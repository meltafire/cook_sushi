namespace Utils.Controllers
{
    public interface IFactory<T> where T : IController
    {
        public T Create();
    }
}