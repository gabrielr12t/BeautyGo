namespace BeautyGo.Domain.Patterns.Patterns.Visitor;

public interface IComponent
{
    void Accept(IVisitor visitor);
}
