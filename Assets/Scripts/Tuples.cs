public class Pair<TF, TS> {

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