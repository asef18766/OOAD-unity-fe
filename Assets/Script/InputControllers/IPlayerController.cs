using UnityEngine;

namespace InputControllers
{
    public interface IPlayerController
    {
        Vector2 OnMove(); // involve jumping
        bool OnClicked();
    }
}
