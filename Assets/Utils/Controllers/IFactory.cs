namespace Utils.Controllers
{
    public interface IFactory<T> where T : Controller
    {
        public T Create();
    }
}