namespace BeautyGo.Domain.Patterns.Visitor;

public interface IVistable<TVisitor>
{
    void Accept(TVisitor visitor);
}
