<Query Kind="Program" />

void Main()
{
	/*
		Since C# doesn't have exhaustive pattern matching, meaning if a certain pattern is not handled,
		it will not cause a compile time error but will throw at run time, which is unlike other languages such as Scala or Haskell.
		It is worth considering that the problems that can be solved using Polymorphic Subtyping might be
		a better option in C# due to the compile time safety gained, when a new subtype/case is added
	*/
	var transactions = new double[]
	{
		10,
		10, 
		20,
		20
	};
	
	var metricServiceV1 = new MetricServiceV1(new MetricCalculator());

	metricServiceV1
		.GetMetrics(transactions, new MetricV1[]
		{
			new SalesMetricV1(),
			new QuantityMetricV1(),
		})
		.Dump();
	
	

	var metricServiceV2 = new MetricServiceV2();

	metricServiceV2
		.GetMetrics(transactions, new MetricV2[]
		{
			new SalesMetricV2(),
			new QuantityMetricV2(),
		})
		.Dump();

}

/*  Pattern Matching Example:
	-	Define a DSL using classes
	- 	Seperate data and functions that manipulate the data
	-	Have an interpreter that can interpret the DSL and give result
*/ 


public class MetricV1 { }

public class SalesMetricV1 : MetricV1 { }

public class QuantityMetricV1 : MetricV1 { }


public interface IMetricCalculator
{
	double CalculateSales(double[] transactions);
	
	double CalculateQuantity(double[] transactions);
}

public class MetricCalculator : IMetricCalculator
{
	public double CalculateSales(double[] transactions)
	{
		return transactions.Sum();
	}

	public double CalculateQuantity(double[] transactions)
	{
		return transactions.Length;
	}
}



public class MetricServiceV1
{
	private readonly IMetricCalculator _metricCalculator;
	
	public MetricServiceV1(IMetricCalculator metricCalculator)
	{
		_metricCalculator = metricCalculator;
	}
	
	public (string, double)[] GetMetrics(double[] transactions, MetricV1[] metrics)
	{
		return metrics.Select(metric =>
		{
			switch (metric)
			{
				case SalesMetricV1 s:
					return ("Sales", _metricCalculator.CalculateSales(transactions));
				case QuantityMetricV1 c:
					return ("Quantity", _metricCalculator.CalculateQuantity(transactions));
				default:
					throw new ArgumentException(
						message: "metric is not a recognized shape",
						paramName: nameof(metric));
			}

		})
		.ToArray();
	}
}



// Polymorphism

/*  Polymorphism/Subtyping Example:
*/


public abstract class MetricV2
{
	public abstract string GetName();
	
	public abstract double Calculate(double[] transactions);
}


public class SalesMetricV2 : MetricV2
{
	public override double Calculate(double[] transactions)
	{
		return transactions.Sum();
	}

	public override string GetName()
	{
		return "Sales";
	}
}


public class QuantityMetricV2 : MetricV2
{
	public override double Calculate(double[] transactions)
	{
		return transactions.Length;
	}

	public override string GetName()
	{
		return "Quantity";
	}
}


public class MetricServiceV2
{
	public (string, double)[] GetMetrics(double[] transactions, MetricV2[] metrics)
	{
		return metrics.Select(metric =>
			(metric.GetName(), metric.Calculate(transactions))
		)
		.ToArray();
	}
}

