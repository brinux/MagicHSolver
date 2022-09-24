using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MgicH
{
	class Program
	{
		static void Main(string[] args)
		{
			var values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			var perms = new Permutations<int>(values);

			var subsetComparer = new SubsestComparer();
			var setsComparer = new SetsComparer();

			Console.WriteLine();
			Console.WriteLine($"Total permutations count: { perms.Count }");

			int solutions = 0;

			var solutionSets = new List<List<List<int>>>();

			long total = perms.Count;
			long count = 0;
			int completion = 0;

			foreach (var p in perms)
			{
				count++;

				if (count * 100 / total > completion)
				{
					completion++;

					Console.WriteLine();
					Console.WriteLine($"Completion: { completion }%");
				}

				/* "H" shape:
				0			1
				2	3	4	5
				6	7	8	9
				10			11
				*/

				var sum = p[0] + p[3] + p[8] + p[11];

				if (
					p[1] + p[4] + p[7] + p[10] == sum &&
					p[0] + p[2] + p[6] + p[10] == sum &&
					p[1] + p[5] + p[9] + p[11] == sum &&
					p[2] + p[3] + p[4] + p[5] == sum &&
					p[6] + p[7] + p[8] + p[9] == sum
				)
				{
					solutions++;

					Console.WriteLine();
					Console.WriteLine($"Solution found with sum: { sum }");
					Console.WriteLine($"{ p[0] }\t\t\t{ p[1]}");
					Console.WriteLine($"{ p[2] }\t{ p[3] }\t{ p[4] }\t{ p[5]}");
					Console.WriteLine($"{ p[6] }\t{ p[7] }\t{ p[8] }\t{ p[9]}");
					Console.WriteLine($"{ p[10] }\t\t\t{ p[11]}");

					var solutionSet = new List<List<int>>();
					solutionSet.Add(new List<int>() { p[0], p[3], p[8], p[11] });
					solutionSet.Add(new List<int>() { p[1], p[4], p[7], p[10] });
					solutionSet.Add(new List<int>() { p[0], p[2], p[6], p[10] });
					solutionSet.Add(new List<int>() { p[1], p[5], p[9], p[11] });
					solutionSet.Add(new List<int>() { p[2], p[3], p[4], p[5] });
					solutionSet.Add(new List<int>() { p[6], p[7], p[8], p[9] });
					solutionSet.ForEach(s => s.Sort());
					solutionSet.Sort(subsetComparer);

					if (!solutionSets.Contains(solutionSet, setsComparer))
					{
						solutionSets.Add(solutionSet);
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine($"Solutions total number: { solutions }");

			Console.WriteLine();
			Console.WriteLine($"Sets total number: { solutionSets.Count }");

			foreach (var set in solutionSets)
			{
				Console.WriteLine();

				foreach (var subset in set)
				{
					Console.WriteLine(string.Join(", ", subset));
				}
			}
		}

		public class SubsestComparer : IComparer<List<int>>
		{
			public int Compare([AllowNull] List<int> x, [AllowNull] List<int> y)
			{
				for (int i = 0; i < x.Count; i++)
				{
					if (x[i] > y[i])
					{
						return 1;
					}
					else if (x[i] < y[i])
					{
						return -1;
					}
				}

				return 0;
			}
		}

		public class SetsComparer : IEqualityComparer<List<List<int>>>
		{
			public static SubsestComparer SubsestComparer = new SubsestComparer();

			public bool Equals([AllowNull] List<List<int>> x, [AllowNull] List<List<int>> y)
			{
				for (int i = 0; i < x.Count; i++)
				{
					var subsetComparison = SubsestComparer.Compare(x[i], y[i]);

					if (subsetComparison != 0)
					{
						return false;
					}
				}

				return true;
			}

			public int GetHashCode([DisallowNull] List<List<int>> set)
			{
				var describer = "";

				foreach (var subset in set)
				{
					describer += string.Join("", subset);
				}

				return describer.GetHashCode();
			}
		}
	}
}
