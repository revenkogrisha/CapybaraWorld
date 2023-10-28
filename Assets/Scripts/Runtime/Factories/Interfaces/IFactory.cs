namespace Core.Factories
{
    public interface IFactory<TProduct>
    {
        public TProduct Create();
    }
}
