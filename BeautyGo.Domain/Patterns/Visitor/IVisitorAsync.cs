namespace BeautyGo.Domain.Patterns.Visitor;

public interface IVisitorAsync<T>
{
    Task Handle(T element);
}
