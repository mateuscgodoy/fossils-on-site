using UnityEngine;

public class FollowPlayerCamp : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] natureSoundClips;
    [SerializeField]
    private Vector2 randomIntervalBetweenSoundClips;

    private GameObject playerGO;
    private Vector3 followPlayerPos;
    private AudioSource cameraAS;
    private float timeCounter, soundInterval;

    private void Awake()
    {
        soundInterval = UnityEngine.Random.Range(randomIntervalBetweenSoundClips.x, randomIntervalBetweenSoundClips.y);
        cameraAS = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        followPlayerPos = transform.position;
    }

    private void Update()
    {
        ReproduceRandomAudioClip();
    }

    private void LateUpdate()
    {
        followPlayerPos.x = playerGO.transform.position.x;
        followPlayerPos.y = playerGO.transform.position.y;

        transform.position = followPlayerPos;
    }

    private void ReproduceRandomAudioClip()
    {
        timeCounter += Time.deltaTime;

        if(timeCounter >= soundInterval && !cameraAS.isPlaying)
        {
            timeCounter = 0;
            soundInterval = Random.Range(randomIntervalBetweenSoundClips.x, randomIntervalBetweenSoundClips.y);
            cameraAS.clip = natureSoundClips[Random.Range(0, natureSoundClips.Length)];
            cameraAS.Play();
        }
    }
}
