using Jili.StatSystem.CardSystem;
using Jili.StatSystem.EntityTree;
using Jili.StatSystem.EntityTree.ConsumableSystem;
using System.Collections.Generic;
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
                List<ScriptableCardData> availableCards = new List<ScriptableCardData>(handle.Result);
                availableCards = FilterAvailableCards(availableCards);

                //c�digo para escolher um card aleat�riamente
                int totalCardsLoaded = availableCards.Count;
                int randomCardIndex = Random.Range(0, totalCardsLoaded);
                tcs.SetResult(availableCards[randomCardIndex]);


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
        CardSelectionScreen.gameObject.SetActive(false);
        playerIdentity.EquipNewCard(currentCard.GetComponent<CardScript>().cardData);
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

    private List<ScriptableCardData> FilterAvailableCards(List<ScriptableCardData> list)
    {
        List<ScriptableCardData> playerCards = playerIdentity.ReadEquippedCards();
        //se o jogador n�o tiver cards equipados, todos os cards de n�vel zero podem ser sorteados.
        // (isso � tempor�rio, at� ter um sorteador adequado para a arma inicial        
        if (playerCards.Count == 0)
        {
            list.RemoveAll(card => card.cardLevel != CardLevel.Zero);
            return list;
        }

        // se o drop de card for espec�fico (arma inicial) apenas cards que satisfa�am a classe do jogador podem ser sorteadas.

        // se o drop de card for especial (boss) apenas cards de uma raridade alta podem ser sorteados.
        
        //Cria uma lista de tags �nicas presentes nos cards dispon�veis
        List<CardCategory> catList = new List<CardCategory>();
        foreach (var card in list)
        {
            if (!catList.Contains(card.cardCategory))
            {
                catList.Add(card.cardCategory);
            }
        }

        //removemos none por ser s� placeholder, stat e atributo por serem gen�ricos e n�o possu�rem n�veis.
        catList.Remove(CardCategory.None);
        catList.Remove(CardCategory.Stat);
        catList.Remove(CardCategory.Attribute);

        foreach (var cat in catList)
        {
            if (!playerCards.Exists(card => card.cardCategory == cat))
            {
                list.RemoveAll(card => (card.cardLevel != CardLevel.Zero) && (card.cardCategory == cat));
            }
            //TODO: se h� um card de n�vel 5 na lista que precisa de uma condi��o espec�fica para poder chegar ao n�vel m�ximo (6), a condi��o deve ser verificada
            if (playerCards.Exists(card => card.cardCategory == cat && card.cardLevel < CardLevel.Max))
            {
                int nextLevel = (int)playerCards.Find(card => card.cardCategory == cat).cardLevel + 1;
                list.RemoveAll(card => card.cardLevel != (CardLevel)nextLevel && card.cardCategory == cat);
            }
            if (playerCards.Exists(card => card.cardCategory == cat && card.cardLevel >= CardLevel.Max)) 
            {
                list.RemoveAll(card => card.cardCategory == cat);
            }
        }
        
        // se for atendida, o card pode ser sorteado, se n�o for o card deve ser ignorado.

        //se todos os cards de n�vel m�ximo de todas as tags est�o na lista, a lista s� pode ter cards gen�ricos.

        //dentre os cards que podem ser sorteados, deve-se fazer um rateio entre as raridades.

        return list;
    }

}
