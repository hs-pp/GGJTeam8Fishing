using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	[SerializeField] Image fadeScreen;
    [SerializeField] AnimationCurve fadeInCurve;
	[SerializeField] float fadeInTime;
    [SerializeField] AnimationCurve fadeOutCurve;
	[SerializeField] float fadeOutTime;
	[SerializeField] string nextSceneToLoad;

	AnimationCurve _animationCurve;
	float _currentTime;
	float _fadeTime;
	Action _onFadeComplete;
	bool _isFading = false;
	string _sceneToLoad;
		
	private void Start()
	{
		FadeInScreen();
	}

	private void Update()
	{
		if (!_isFading) return;

		_currentTime += Time.deltaTime;

		Color color = fadeScreen.color;
		color.a = _animationCurve.Evaluate(Mathf.Clamp01(_currentTime / _fadeTime));
		fadeScreen.color = color;

		if (_currentTime > _fadeTime)
		{
			_onFadeComplete?.Invoke();
			_isFading = false;
		}
	}

	private void FadeInScreen()
	{
		_animationCurve = fadeInCurve;
		_currentTime = 0.0f;
		_fadeTime = fadeInTime;
		_isFading = true;
		_onFadeComplete = null;
	}

	public void LoadNextScene()
	{
		_animationCurve = fadeOutCurve;
		_currentTime = 0.0f;
		_fadeTime = fadeOutTime;
		_isFading = true;
		_sceneToLoad = nextSceneToLoad;
		_onFadeComplete = LoadScene;
	}

	public void LoadScene(string sceneName)
	{
		_animationCurve = fadeOutCurve;
		_currentTime = 0.0f;
		_fadeTime = fadeOutTime;
		_isFading = true;
		_sceneToLoad = sceneName;
		_onFadeComplete = LoadScene;
	}

	public void LoadFishingScene()
	{
		_animationCurve = fadeOutCurve;
		_currentTime = 0.0f;
		_fadeTime = fadeOutTime;
		_isFading = true;
		_sceneToLoad = "Fishing";
		_onFadeComplete = LoadScene;
	}

	private void LoadScene()
	{
		SceneManager.LoadScene(_sceneToLoad);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
