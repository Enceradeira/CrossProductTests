using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace UnitTestProject1
{
	[TestFixture]
	public class UnitTest1
	{
		private IEnumerable<KontoSaldo> AddEmptyKontoSaldoIfKontoNotExists(IEnumerable<KontoSaldo> kontoSaldi,
			IEnumerable<KontoSaldo> otherKontoSaldi)
		{
			var allKontoSaldi =
				kontoSaldi.Select(k => k.Konto)
					.Concat(otherKontoSaldi.Select(k => k.Konto))
					.Distinct()
					.Select(k => new KontoSaldo {Konto = k, Saldo = 0});

			return from allKontoSaldo in allKontoSaldi
				join kontoSaldo in kontoSaldi on allKontoSaldo.Konto equals kontoSaldo.Konto into kontoJoin
				from kontoOrEmptyKonto in kontoJoin.DefaultIfEmpty()
				select new KontoSaldo
				{
					Konto = kontoOrEmptyKonto != null ? kontoOrEmptyKonto.Konto : allKontoSaldo.Konto,
					Saldo = kontoOrEmptyKonto != null ? kontoOrEmptyKonto.Saldo : 0m,
				};
		}

		private void AddEmptyKontoSaldoIfKontoNotExists(List<KontoSaldo> kontoSaldi,
			List<KontoSaldo> otherKontoSaldi)
		{
			var allKontoSaldi =
				kontoSaldi.Select(k => k.Konto)
					.Concat(otherKontoSaldi.Select(k => k.Konto))
					.Distinct()
					.Select(k => new KontoSaldo { Konto = k, Saldo = 0 });


			var result = from allKontoSaldo in allKontoSaldi
				   join kontoSaldo in kontoSaldi on allKontoSaldo.Konto equals kontoSaldo.Konto into kontoJoin
				   from kontoOrEmptyKonto in kontoJoin.DefaultIfEmpty()
				   select new KontoSaldo {
					   Konto = kontoOrEmptyKonto != null ? kontoOrEmptyKonto.Konto : allKontoSaldo.Konto,
					   Saldo = kontoOrEmptyKonto != null ? kontoOrEmptyKonto.Saldo : 0m,
				   };
			var resultEvaluated = result.ToArray();

			kontoSaldi.Clear();
			kontoSaldi.AddRange(resultEvaluated);
		}

		[Test]
		public void AddEmptyKontoSaldoIfKontoNotExistsUsingFunctionalApproach()
		{
			var year2013 = new[]
			{
				new KontoSaldo() {Konto = "1", Saldo = 34.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.3m},
				new KontoSaldo() {Konto = "6", Saldo = 14.3m},
				new KontoSaldo() {Konto = "10", Saldo = 12.5m}
			};

			var year2014 = new[]
			{
				new KontoSaldo() {Konto = "1", Saldo = 12.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.5m},
				new KontoSaldo() {Konto = "4", Saldo = 4m},
				new KontoSaldo() {Konto = "5", Saldo = 12.5m},
				new KontoSaldo() {Konto = "10", Saldo = 75.02m},
				new KontoSaldo() {Konto = "11", Saldo = 2.30m}
			};


			var year2013Concatenated = AddEmptyKontoSaldoIfKontoNotExists(year2013, year2014);

			Assert.That(year2013Concatenated, Is.EquivalentTo(new[]
			{
				new KontoSaldo() {Konto = "1", Saldo = 34.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.3m},
				new KontoSaldo() {Konto = "4", Saldo = 0},
				new KontoSaldo() {Konto = "5", Saldo = 0},
				new KontoSaldo() {Konto = "6", Saldo = 14.3m},
				new KontoSaldo() {Konto = "10", Saldo = 12.5m},
				new KontoSaldo() {Konto = "11", Saldo = 0},
			}));
		}

		[Test]
		public void AddEmptyKontoSaldoIfKontoNotExistsUsingSideEffect()
		{
			var year2013 = new List<KontoSaldo>
			{
				new KontoSaldo() {Konto = "1", Saldo = 34.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.3m},
				new KontoSaldo() {Konto = "6", Saldo = 14.3m},
				new KontoSaldo() {Konto = "10", Saldo = 12.5m}
			};

			var year2014 = new List<KontoSaldo>
			{
				new KontoSaldo() {Konto = "1", Saldo = 12.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.5m},
				new KontoSaldo() {Konto = "4", Saldo = 4m},
				new KontoSaldo() {Konto = "5", Saldo = 12.5m},
				new KontoSaldo() {Konto = "10", Saldo = 75.02m},
				new KontoSaldo() {Konto = "11", Saldo = 2.30m}
			};


			AddEmptyKontoSaldoIfKontoNotExists(year2013, year2014);

			Assert.That(year2013, Is.EquivalentTo(new[]
			{
				new KontoSaldo() {Konto = "1", Saldo = 34.5m},
				new KontoSaldo() {Konto = "2", Saldo = 12.3m},
				new KontoSaldo() {Konto = "4", Saldo = 0},
				new KontoSaldo() {Konto = "5", Saldo = 0},
				new KontoSaldo() {Konto = "6", Saldo = 14.3m},
				new KontoSaldo() {Konto = "10", Saldo = 12.5m},
				new KontoSaldo() {Konto = "11", Saldo = 0},
			}));
		}
	}
}