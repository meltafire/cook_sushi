namespace Utils.Controllers
{
    public interface IFactoryWithData<T, U> where T : Controller where U : FactoryData
    {
        public T Create(U data);
    }
}