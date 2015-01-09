using System;

namespace UnitTestProject1
{
	public class KontoSaldo
	{
		public string Konto { get; set; }

		public decimal Saldo { get; set; }

		protected bool Equals(KontoSaldo other)
		{
			return Equals(Konto, other.Konto) && Saldo == other.Saldo;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((KontoSaldo) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Konto != null ? Konto.GetHashCode() : 0)*397) ^ Saldo.GetHashCode();
			}
		}

		public override string ToString()
		{
			return string.Format("Konto={0}, Saldo={1}", Konto, Saldo);
		}
	}
}