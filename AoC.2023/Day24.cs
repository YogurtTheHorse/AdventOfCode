using AoC.Library.Runner;
using AoC.Library.Utils;

namespace AoC._2023;

using Point = PointBase<double>;

[DateInfo(2023, 24, AdventParts.PartOne)]
public class Day24 : AdventSolution
{
    public double MinV { get; set; } = 200000000000000;

    public double MaxV { get; set; } = 400000000000000;

    public Line BottomLine => new(
        new Point(MinV, MinV),
        new Point(MinV, MaxV)
    ); 
    public Line LeftLine => new(
        new Point(MinV, MinV),
        new Point(MaxV, MinV)
    ); 
    public Line TopLine => new(
        new Point(MinV, MaxV),
        new Point(MaxV, MaxV)
    ); 
    public Line RightLine => new(
        new Point(MaxV, MinV),
        new Point(MaxV, MaxV)
    );

    public override object SolvePartOne()
    {
        if (Input.Raw.StartsWith("19"))
        {
            MinV = 7;
            MaxV = 27;
        }

        var lines = Input.Lines.Select(ParseLine).Where(Helpers.NotNull).Cast<Line>().ToArray();
        var cnt = 0;

        for (int i = 0; i < lines.Length; i++)
        for (int j = 0; j < i; j++)
            if (Linecast(lines[i].Start, lines[i].End, lines[j], false).HasValue)
                cnt++;

        return cnt;
    }

    private Line? ParseLine(string l, int i)
    {
        var (starts, vels) = l.Replace(" ", "").SmartSplit('@').Unpack2();
        var (xs, ys, zs) = starts.SmartSplit(',').ToLong().Unpack3();
        var (xv, yv, zv) = vels.SmartSplit(',').ToLong().Unpack3();

        var start = new Point(xs, ys);
        var dir = new Point(xv, yv);

        var btm = Linecast(start, dir, BottomLine, true);
        var left = Linecast(start, dir, LeftLine, true);
        var top = Linecast(start, dir, TopLine, true);
        var right = Linecast(start, dir, RightLine, true);

        List<Point> pts = new();

        if (btm.HasValue) pts.Add(btm.Value);
        if (left.HasValue) pts.Add(left.Value);
        if (top.HasValue) pts.Add(top.Value);
        if (right.HasValue) pts.Add(right.Value);

        if (pts.Count != 2)
        {
            if (start.X >= MinV && start.X <= MaxV && start.Y >= MinV && start.Y <= MaxV)
            {
                pts.Add(start);
            }
                
            return pts.Count == 2 ? new Line(pts[0], pts[1], i) : null;
        }

        return new Line(pts[0], pts[1], i);
    }

    public override object SolvePartTwo() => throw new NotImplementedException();

    public record Line(Point Start, Point End, int Id = -1);
    
    public Point? Linecast(Point origin, Point direction, Line line, bool isRay)
    {
        var realDirection = isRay
            ? direction
            : (direction - origin);
        
        double rayToLineStartX = line.Start.X - origin.X;
        double rayToLineStartY = line.Start.Y - origin.Y;
        double lineEndToStartX = line.End.X - line.Start.X;
        double lineEndToStartY = line.End.Y - line.Start.Y;
        double denominator = realDirection.X * lineEndToStartY - realDirection.Y * lineEndToStartX;

        if (denominator == 0)
        {
            return null;
        }

        double u = (rayToLineStartX * lineEndToStartY - rayToLineStartY * lineEndToStartX) / denominator;
        double t = (rayToLineStartX * realDirection.Y - rayToLineStartY * realDirection.X) / denominator;

        if (u >= 0 && (isRay || u <= 1) && t is >= 0 and <= 1)
        {
            return new Point(origin.X + u * realDirection.X, origin.Y + u * realDirection.Y);
        }

        return null;
    }
}