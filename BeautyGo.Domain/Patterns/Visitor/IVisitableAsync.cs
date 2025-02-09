namespace BeautyGo.Domain.Patterns.Visitor;

public interface IVisitableAsync<TVisitor>
{
    Task Accept(TVisitor visitor);
}
