using System;

class Pair<F,S> {

	public F First { get; set; }
	public S Second { get; set; }

	public Pair(F first, S second) {
		this.First = first;
		this.Second = second;
	}

	public override bool Equals(object obj) {
		if (Object.ReferenceEquals(this, obj)) {
			return true;
		}
		Pair<F, S> instance = obj as Pair<F, S>;
		if (instance == null) {
			return false;
		}
		return this.First.Equals(instance.First) && this.Second.Equals(instance.Second);
	}

	public override int GetHashCode() {
		return this.First.GetHashCode() ^ this.Second.GetHashCode();
	}
}