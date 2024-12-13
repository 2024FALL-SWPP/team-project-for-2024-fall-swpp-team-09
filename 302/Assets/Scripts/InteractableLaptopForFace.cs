public class InteractableLaptopForFace : InteractableLaptop
{
    /**************************************
     * implementation: InteractableLaptop *
     **************************************/

    public override void OnInteract()
    {
        Anomaly02_Laptop obj = GetComponentInParent<Anomaly02_Laptop>();

        base.OnInteract();

        if (obj != null && obj.enabled) {
            obj.OnInteract();
        }
    }
}
