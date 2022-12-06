Доработка интерактивного приложения и его подготовка к сборке
Отчет по лабораторной работе #4 выполнил(а):
- Кулаков Иван Александрович
- РИ300003

| Задание | Выполнение | Баллы |
| ------ | ------ | ------ |
| Задание 1 | * |   60 |
| Задание 2 | * |   20 |
| Задание 3 | * |   20 |

## Цель работы
подготовить разрабатываемое интерактивное приложение к сборке и публикации.
## Задание 1
### Используя видео-материалы практических работ 1-5 повторить реализацию игровых механик:
### - 1 Практическая работа «Создание анимации объектов на сцене».
### – 2 Практическая работа «Создание стартовой сцены и переключение между ними».
### - 3 Практическая работа «Доработка меню и функционала с остановкой игры».
### - 4 Практическая работа «Добавление звукового сопровождения в игре».
### - 5 Практическая работа «Добавление персонажа и сборка сцены для публикации на web-ресурсе».
Ход работы:
- Создаём на сцене объект облако
- Делаем дубликат аниматора облака из пакета
- Создаём анимацию движения для облака, добавляем её в аниматор
- Аниматор присоединяем к облаку
- На сцену меню добавляем дракона, задаём ему анимацию idle01 из набора
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/MenuAnimation.gif)
- На стартовую сцену добавляем меню, пишем скрипт с 2-мя функциями для переключения, вешаем функционал на кнопки
- Настройки добавляем в качестве скрываемого меню на стартовую сцену
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/StartingScene.PNG)
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/PlayButton.PNG)
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/OptionButton.PNG)
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/OptionScene.PNG)



        public class MainMenu : MonoBehaviour
        {
            public void PlayGame(){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            public void QuitGame(){
                Application.Quit();
            }
        }


- Добавляем обе сцены в сборку
- Для игровой сцены делаем скрипт паузы и добвляем его в камеру
- Скрипт останавливает течение игры
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/PauseScene.PNG)



        public class Pause : MonoBehaviour
        {
            private bool paused = false;
            public GameObject panel;

            void Update()
            {
                if(Input.GetKey(KeyCode.Space)){
                    if (!paused){
                        Time.timeScale = 0;
                        paused = true;
                        panel.SetActive(true);
                    }
                    else{
                        Time.timeScale = 1;
                        paused = false;
                        panel.SetActive(false);
                    }
                }
                if(Input.GetKeyDown(KeyCode.Escape)){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
            }
        }
        
        
- Добавляем в игру музыку, вешая AudioSource на камеру
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/Music.PNG)
- Добавляем звуки контакта яйца с щитом и взрыва яйца при контакет с землёй(включаются в скрипте щита и яйца соотвественно)



        public class EnergyShield : MonoBehaviour
        {
            public TextMeshProUGUI scoreGT;
            public AudioSource audioSource;

            void Start() {
                GameObject scoreGO = GameObject.Find("Score");
                scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
                scoreGT.text = "0";
            }

            void Update() {
                Vector3 mousePos2D = Input.mousePosition;
                mousePos2D.z = -Camera.main.transform.position.z;
                Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
                Vector3 pos = this.transform.position;
                pos.x = mousePos3D.x;
                this.transform.position = pos;
            }

            private void OnCollisionEnter(Collision coll) {
                GameObject Collided = coll.gameObject;
                if(Collided.tag == "Dragon Egg"){
                    Destroy(Collided);
                }
                int score = int.Parse(scoreGT.text);
                score +=1;
                scoreGT.text = score.ToString();

                audioSource = GetComponent<AudioSource>();
                audioSource.Play();
            }
        }
        
        
        
       public class DragonEgg : MonoBehaviour
        {
            public static float bottomY = -30f;
            public AudioSource audioSource;

            void Start()
            {

            }

            private void OnTriggerEnter(Collider other) {
                ParticleSystem ps = GetComponent<ParticleSystem>();
                var em = ps.emission;
                em.enabled = true;

                Renderer rend;
                rend = GetComponent<Renderer>();
                rend.enabled = false;

                audioSource = GetComponent<AudioSource>();
                audioSource.Play();
            }

            // Update is called once per frame
            void Update()
            {
                if(transform.position.y < bottomY){
                    Destroy(this.gameObject);
                    DragonPicker apScript = Camera.main.GetComponent<DragonPicker>();
                    apScript.DragonEggDestroyd();
                }
            }
        } 
        

## Задание 2
### Привести описание того, как происходит сборка проекта проекта под другие платформы. Какие могут быть особенности?
## Задание 3
### Добавить в меню Option возможность изменения громкости (от 0 до 100%) фоновой музыки в игре.
- Создаём в меню настроек слайдер, привязываем к его изменению изменение параметра Volume в AudioSource
![image](https://github.com/Snoubort/Game-services-lab4/blob/main/MatForReadMe/Music.PNG)
