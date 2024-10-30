using UnityEngine;

namespace AstroNut.Characters.Player
{
    /// <summary>
    /// Base class of the player where all the components are being kept.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        // Core components
        private Rigidbody2D _rigidbody;
        private PlayerController _playerController;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            _playerController.OnInputRotated();
        }

        private void FixedUpdate()
        {
            _playerController.OnInputThrust(_rigidbody);
        }
    }
}
