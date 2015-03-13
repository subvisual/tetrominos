class Pair<TF, TS> {

	public TF First { get; set; }
	public TS Second { get; set; }

	public Pair(TF first, TS second) {
		First = first;
		Second = second;
	}

	public override bool Equals(object obj) {
		if (ReferenceEquals(this, obj)) {
			return true;
		}
		var instance = obj as Pair<TF, TS>;
		if (instance == null) {
			return false;
		}
		return First.Equals(instance.First) && Second.Equals(instance.Second);
	}

	public override int GetHashCode() {
		return First.GetHashCode() ^ Second.GetHashCode();
	}
}

class Triple<TF, TS, TT> {

	public TF First { get; set; }
	public TS Second { get; set; }
	public TT Third { get; set; }

	public Triple(TF first, TS second, TT third) {
		First = first;
		Second = second;
		Third = third;
	}

	public override bool Equals(object obj) {
		if (ReferenceEquals(this, obj)) {
			return true;
		}
		var instance = obj as Triple<TF, TS, TT>;
		if (instance == null) {
			return false;
		}
		return First.Equals(instance.First) && Second.Equals(instance.Second) && Third.Equals(instance.Third);
	}

	public override int GetHashCode() {
		return First.GetHashCode() ^ Second.GetHashCode() ^ Third.GetHashCode();
	}

	public Pair<TF, TS> PairXY() {
		return new Pair<TF, TS>(First, Second);
	}
}