export class JumpType {

    static Belly = new JumpType(0);
    static Freefly = new JumpType(1);
    static Wingsuit = new JumpType(2);
    static Highpull = new JumpType(3);
    static Tandem = new JumpType(4);
    static AFF = new JumpType(5);
    static CRW = new JumpType(6);
    static XRW = new JumpType(7);

    static JumpNumberStringMap = new Map([
        [JumpType.Belly.value, "Belly"],
        [JumpType.Freefly.value, "Freefly"],
        [JumpType.Wingsuit.value, "Wingsuit"],
        [JumpType.Highpull.value, "Highpull"],
        [JumpType.Tandem.value, "Tandem"],
        [JumpType.AFF.value, "AFF"],
        [JumpType.CRW.value, "CRW"],
        [JumpType.XRW.value, "XRW"]
    ]);

    constructor(value) {
        this.value = value;
    }

    static jumpTypeToString(value) {
        if (value > JumpType.JumpNumberStringMap.size || value < 0) {
            return "Unknown";
        }
        return JumpType.JumpNumberStringMap.get(value);
    }
}