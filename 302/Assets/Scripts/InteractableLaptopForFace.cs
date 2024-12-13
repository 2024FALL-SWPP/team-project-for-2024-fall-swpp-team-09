public class InteractableLaptopForFace : InteractableLaptop
{
    /**************************************
     * implementation: InteractableLaptop *
     **************************************/

    public override void OnInteract()
    {
        Anomaly02Laptop obj = GetComponentInParent<Anomaly02Laptop>();

        base.OnInteract();

        if (obj != null && obj.enabled) {
            obj.OnInteract();
        }
    }
}
