using Jili.StatSystem.CardSystem;
using Jili.StatSystem.EntityTree;
using Jili.StatSystem.EntityTree.ConsumableSystem;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardScreen : MonoBehaviour
{
    public GameObject CardSelectionScreen;
    public GameObject CardTemplate;
    private PlayerIdentity playerIdentity;

    private Transform menuBackground;
    private TextMeshProUGUI chanceCounter;
    private GameObject currentCard;

    private int cardPickAmount;
    private int cardPickCounter;
    private int maxCardReject;
    private int cardRejectCounter;

    private bool isShowing = false;

    private void Awake()
    {
        menuBackground = CardSelectionScreen.transform.Find("Background");
        chanceCounter = CardSelectionScreen.transform.Find("ChanceCounter").GetComponentInChildren<TextMeshProUGUI>();
        playerIdentity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIdentity>();

        CardSelectionScreen.SetActive(false);
    }

    void OnEnable()
    {
        ConsumableUse.OpenCardPacket += HandleOpenCardPacket;
    }

    private void OnDestroy()
    {
        ConsumableUse.OpenCardPacket -= HandleOpenCardPacket;
    }

    private async void HandleOpenCardPacket(int amount)
    {
        isShowing = true;
        maxCardReject = 5;
        cardRejectCounter = 0;
        chanceCounter.text = (maxCardReject + "/" + maxCardReject + " Cards Dispon�veis");

        if (currentCard != null)
        {
            Destroy(currentCard);
        }

        await CreateCardObject();
        CardSelectionScreen.SetActive(true);

        // Pausa o tempo do jogo
        Time.timeScale = 0;

        // Aguarda at� que o jogador feche o menu
        await WaitForCardClose();

        // Retorna o tempo do jogo ao normal
        Time.timeScale = 1;
    }

    private async Task WaitForCardClose()
    {
        // Aguarda enquanto a janela de level-up est� ativa
        while (isShowing)
        {
            await Task.Yield();
        }
    }

    private async Task CreateCardObject()
    {
        var cardData = await LoadCardData();

        GameObject newCard = Instantiate(CardTemplate, menuBackground);
        CardScript cardScript = newCard.GetComponent<CardScript>();
        cardScript.cardData = cardData;

        currentCard = newCard;
    }

    private async Task<ScriptableCardData> LoadCardData()
    {
        var tcs = new TaskCompletionSource<ScriptableCardData>();

        Addressables.LoadAssetsAsync<ScriptableCardData>("Card Data", null).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {

                //c�digo para escolher um card aleat�riamente
                int totalCardsLoaded = handle.Result.Count;
                int randomCardIndex = Random.Range(0, totalCardsLoaded);
                tcs.SetResult(handle.Result[randomCardIndex]);
            }
            else
            {
                tcs.SetException(new System.Exception("Falha ao carregar GameObjects Addressables"));
            }
        };

        return await tcs.Task; ;

    }

    public void AcceptCardButton()
    {
        /*  Exemplo de l�gica a ser implementada no futuro para quando
         *  mais de um card puder ser escolhido.
        if (cardPickCounter >= cardPickAmount)
        {
            return;
        }
        else
        {
            cardPickCounter++;
            playerIdentity.AddCard(currentCard.GetComponent<CardScript>().cardData);
            Destroy(currentCard);
            _ = CreateCardObject();
        }*/

        isShowing = false;
        playerIdentity.EquipNewCard(currentCard.GetComponent<CardScript>().cardData);
        CardSelectionScreen.gameObject.SetActive(false);
    }

    public async void RejectCardButton()
    {
        if (cardRejectCounter >= maxCardReject)
        {
            return;
        }
        else
        {
            cardRejectCounter++;
            Destroy(currentCard);
            await CreateCardObject();
        }

        chanceCounter.text = (maxCardReject - cardRejectCounter + "/" + maxCardReject + " Cards Dispon�veis");
    }

}
