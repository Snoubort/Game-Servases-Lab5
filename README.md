Создание индивидуальной системы достижения пользователя и ее интеграция в пользовательский интерфейс
Отчет по лабораторной работе #5 выполнил(а):
- Кулаков Иван Александрович
- РИ300003

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * |   60 |
| Задание 2 | * |   20 |
| Задание 3 | # |   20 |

## Цель работы
создание интерактивного приложения с рейтинговой системой пользователя и интеграция игровых сервисов в готовое приложение.
## Задание 1
### Используя видео-материалы практических работ 1-5 повторить реализацию игровых механик:
### - 1 Практическая работа «Интеграции авторизации с помощью Яндекс SDK».
### – 2 Практическая работа «Сохранение данных пользователя на платформе Яндекс Игры».
### - 3 Практическая работа «Сбор данных об игроке и вывод их в интерфейсе».
### - 4 Практическая работа «Интеграция таблицы лидеров».
### - 5 Практическая работа «Интеграция системы достижений в проект».
Ход работы:
- создаём на сцене объект YandexManager, затем на него вешаем новый скрипт CheckConnectYG.
- В скриптемы проверяем подключение SDK и в случае успеха - проверяем авторизацию



        public class CheckConnectYG : MonoBehaviour
            {
            private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
            private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
            private TextMeshProUGUI scoreBest;
            public GameObject AchivList;
            // Start is called before the first frame update
            void Start(){   
                Debug.Log(YandexGame.SDKEnabled);
                if (YandexGame.SDKEnabled){
                    CheckSDK();
                }
            }

            // Update is called once per frame
            void Update()
            {

            }

            public void CheckSDK(){
                if(YandexGame.auth){
                    Debug.Log("User authorization OK");
                }
                else{
                    Debug.Log("User NOT authorization");
                    YandexGame.AuthDialog();
                }
                GameObject scoreBO = GameObject.Find("BestScore");
                scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
                scoreBest.text = "Best Score: " + YandexGame.savesData.bestScore.ToString();
                if ((YandexGame.savesData.achivment)[0] == null){

                }
                else{
                    foreach(string value in YandexGame.savesData.achivment){
                        AchivList.GetComponent<TextMeshProUGUI>().text=AchivList.GetComponent<TextMeshProUGUI>                                      ().text+value+"\n";                                 
                    }
                }
            }
    }




- В скрипте от YG SavesYG создаём переменные для наших сохранений, после чего в DragonPicker создаём переменные и 2 метада, один из которых подгружает сохранения, а второй - наоборотсохраняет
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/PauseScene.PNG)



        namespace YG
        {
            [System.Serializable]
            public class SavesYG
            {
                public bool isFirstSession = true;
                public string language = "ru";
                public bool feedbackDone;
                public bool promptDone;

                // Ваши сохранения
                public int score;
                public int bestScore = 0;
                public string[] achivment;
            }
        }

        public class DragonPicker : MonoBehaviour
        {
            private void OnEnable() => YandexGame.GetDataEvent += GetLoadSave;
            private void OnDisable() => YandexGame.GetDataEvent -= GetLoadSave;
            public GameObject energyShieldPrefab;
            public int numEnergyShield = 3;
            public float energyShieldBottomY = -6f;
            public float energyShieldRadius = 1.5f;
            public TextMeshProUGUI scoreGT;
            private TextMeshProUGUI playerName;

            public List<GameObject> shieldList;

            public AudioSource backgroundMusic;

            void Start()
            {
                if(YandexGame.SDKEnabled){
                    GetLoadSave();
                }
                backgroundMusic.volume = Dataholder.soundLevel;

                shieldList = new List<GameObject>();
                for (int i = 1; i<= numEnergyShield; i++){
                    GameObject tShieldGo = Instantiate<GameObject>(energyShieldPrefab);
                    tShieldGo.transform.position = new Vector3(0, energyShieldBottomY, 0);
                    tShieldGo.transform.localScale = new Vector3(1*i, 1*i, 1*i);
                    shieldList.Add(tShieldGo);
                }
            }

            // Update is called once per frame
            void Update()
            {

            }

            public void DragonEggDestroyd(){
                GameObject[] tDragonEggArray = GameObject.FindGameObjectsWithTag("Dragon Egg");
                foreach (GameObject tGO in tDragonEggArray){
                    Destroy(tGO);
                }

                int shieldIndex = shieldList.Count -1;
                GameObject tShieldGo = shieldList[shieldIndex];
                shieldList.RemoveAt(shieldIndex);
                Destroy(tShieldGo);

                if (shieldList.Count == 0){
                    GameObject scoreGO = GameObject.Find("Score");
                    scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
                    string[] achivList;
                    achivList = YandexGame.savesData.achivment;
                    achivList[0] = "Береги щиты";
                    UserSave(int.Parse(scoreGT.text), YandexGame.savesData.bestScore, achivList);
                    YandexGame.NewLeaderboardScores("TOPPlayerScore", int.Parse(scoreGT.text));
                    SceneManager.LoadScene("_0Scene");
                    GetLoadSave();
                }
            }

            public void GetLoadSave(){
                Debug.Log(YandexGame.savesData.score);
                GameObject playerNamePrefabGUI = GameObject.Find("PlayerName");
                playerName = playerNamePrefabGUI.GetComponent<TextMeshProUGUI>();
                playerName.text = YandexGame.playerName;
            }

            public void UserSave(int currentScore, int currentBestScore, string[] currentAchiv){
                YandexGame.savesData.score = currentScore;
                if(currentScore > currentBestScore){
                    YandexGame.savesData.bestScore = currentScore;
                }
                YandexGame.savesData.achivment = currentAchiv;
                YandexGame.SaveProgress();
            }
        }

        
        
- Добавляем лидерборд в игру, после чего добавляем его же в панеле разработчика(см скрипт DragonPicker)
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/Music.PNG)
- Добавляем систему ачивок, создав массив строк в сохраняемых переменных YG и сделав его перенос в игру в CheckConnectYG   

## Задание 2
### Описать не менее трех дополнительных функций Яндекс SDK, которые могут быть интегрированы в игру.
- Обычная и видео реклама, смена языков, внутриигровой магазин, система отзывов
## Задание 3
### Добавить в меню Option возможность изменения громкости (от 0 до 100%) фоновой музыки в игре.
- Создаём в меню настроек слайдер, привязываем к его изменению изменение параметра Volume в AudioSource
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/Music.PNG)
