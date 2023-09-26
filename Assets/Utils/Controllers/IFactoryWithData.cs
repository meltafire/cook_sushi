namespace Utils.Controllers
{
    public interface IFactoryWithData<T, U> where T : IController where U : FactoryData
    {
        public T Create(U data);
    }
}