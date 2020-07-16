using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    [Header("Gameplay")]
    public float m_ElapsedTime;

    [Header("UI")]
    public Text m_TrialTimeUI;
    public Text m_HighScoreUI;
    public Text m_ScoreGenUI;
    public Text m_SurvivorsUI;
    public Text m_GenerationUI;

    [Header("Population")]
    public GameObject m_SushiAIPrefab;
    public Transform m_Spawn;
    public int m_PopulationSize = 100;
    public float m_GenerationTime = 60.0f;
    public Transform m_SushiParent;
    [HideInInspector]
    private float m_Fitness;
    [HideInInspector]
    public float m_MaxFitness;
    [HideInInspector]
    public List<GameObject> m_Population = new List<GameObject>();

    [Header("Evolution")]
    public float m_ElitismPercent = 5;
    public int m_ElitismNumber;
    public int m_TournamentSelectionNumber = 2;
    public float m_MutationRate = 0.02f;

    private int m_Generation = 1;
    private int m_Survivors;

    [Header("Camera")]
    public CinemachineVirtualCamera vcam;

    private void Start()
    {
        if (!vcam) vcam = GameObject.FindWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>();

        for (int i = 0; i < m_PopulationSize; i++) {
            var sushi = Instantiate(m_SushiAIPrefab, m_Spawn.position, Quaternion.identity, m_SushiParent);
            var brain = sushi.GetComponent<Brain>();
            for (int j = 0; j < brain.m_Weights.Length; j++) {
                brain.m_Weights[j] = Random.Range(-1.0f, 1.0f);
            }
            m_Population.Add(sushi);
        }
        m_ElitismNumber = (int)(m_PopulationSize * (m_ElitismPercent / 100.0f));
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        m_Survivors = m_Population.Where(x => !x.GetComponent<Brain>().m_Dead).ToList().Count();
        m_Fitness = m_Population.Max(x => x.GetComponent<Brain>().m_Fitness);

        vcam.Follow = m_Population.Where(x => !x.GetComponent<Brain>().m_Dead).OrderByDescending(x => x.GetComponent<Brain>().m_Fitness).First().transform;

        if (m_ElapsedTime >= m_GenerationTime || m_Survivors == 0) {
            NewPopulation();
            m_ElapsedTime = 0.0f;
        }

        UpdateUI();
    }

    public float[] Mutate(float[] weights, float mutationRate)
    {
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = Random.Range(0.0f, 1.0f) < mutationRate ? Random.Range(0.0f, 1.0f) : weights[i];
        }
        return weights;
    }

    public float[] Crossover(float[] parent1, float[] parent2)
    {
        float[] offspring = new float[parent1.Length];

        for (int i = 0; i < offspring.Length; i++) {
            offspring[i] = parent1[i] + 0.01f * (parent2[i] - parent1[i]);
        }

        return offspring;
    }

    public GameObject TournamentSelection(int numberToSelect)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < numberToSelect; i++) {
            int index = Random.Range(0, m_PopulationSize);
            list.Add(m_Population[index]);
        }
        List<GameObject> sortedList = list.OrderByDescending(x => x.GetComponent<Brain>().m_Fitness).ToList();
        return sortedList[0];
    }

    public void NewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();

        // Elitismo
        List<GameObject> sortedPopulation = m_Population.
            OrderByDescending(
                x => x.GetComponent<Brain>().m_Fitness
            )
            .ToList();

        for (int i = 0; i < m_ElitismNumber; i++) {
            var weights = sortedPopulation[i].GetComponent<Brain>().m_Weights;

            var sushi = Instantiate(m_SushiAIPrefab, m_Spawn.position, Quaternion.identity, m_SushiParent);
            var brain = sushi.GetComponent<Brain>();
            System.Array.Copy(weights, brain.m_Weights, weights.Length);

            newPopulation.Add(sushi);
        }

        while (newPopulation.Count < m_PopulationSize) {
            var parent1 = TournamentSelection(m_TournamentSelectionNumber).GetComponent<Brain>();
            var parent2 = TournamentSelection(m_TournamentSelectionNumber).GetComponent<Brain>();

            var offspring = Crossover(parent1.m_Weights, parent2.m_Weights);
            offspring = Mutate(offspring, m_MutationRate);

            var sushi = Instantiate(m_SushiAIPrefab, m_Spawn.position, Quaternion.identity, m_SushiParent);
            var brain = sushi.GetComponent<Brain>();
            brain.m_Weights = offspring;

            newPopulation.Add(sushi);
        }

        foreach (GameObject gameObject in m_Population) {
            Destroy(gameObject);
        }

        m_Generation++;
        m_Population = newPopulation;
    }

    private void UpdateUI()
    {
        m_TrialTimeUI.text = $"<size=45>Trial Time: {m_ElapsedTime}</size>";
        m_HighScoreUI.text = $"<size=45>High Score: {m_MaxFitness}</size>";
        m_ScoreGenUI.text = $"<size=45>Score: {m_Fitness}</size>";
        m_SurvivorsUI.text = $"<size=45>Survivors: {m_Survivors}</size>";
        m_GenerationUI.text = $"<size=45>Generation: {m_Generation}</size>";
    }
}
