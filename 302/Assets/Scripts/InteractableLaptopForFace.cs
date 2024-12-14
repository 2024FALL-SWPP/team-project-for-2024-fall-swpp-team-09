public class InteractableLaptopForFace : InteractableLaptop
{
    /**************************************
     * implementation: InteractableLaptop *
     **************************************/

    public override void OnInteract()
    {
        Anomaly2_Laptop obj = GetComponentInParent<Anomaly2_Laptop>();

        base.OnInteract();

        if (obj != null && obj.enabled) {
            obj.OnInteract();
        }
    }
}
