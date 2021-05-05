using System;
using System.IO;
using System.Linq;

namespace ConsoleApp6FindUniqString {
	class Program {
		static void Main(string[] args) {
			string path = @"TextFile1.txt";

			// Первый индекс указывает на строку выше
			var aIndex = 0;
			// Второй индекс идёт вперёд и указывает последующие строки
			var bIndex = 0;
			// Уникальных строк найдено
			var uniqStrResult = 0;

			using (StreamReader sr = new StreamReader(path)) {
				
				// Пременные для считываения из stream
				char[] a = null;
				char[] b = null;


				// Позиционируем второй индекс,
				// для этого находим первый разрыв
				while (sr.Peek() >= 0) {
					b = new char[1];
					sr.Read(b, bIndex++, b.Length);

					if(b.First() == '\n') {
						break;
					}
				}

				if(sr.Peek() < 0) {
					// Случай когда в файле единственна строка, считаем ее уникальной
					uniqStrResult = 1;
					return;
				}

				int startAString = 0;
				int currentAStringNumber = 0;
				int currentBStringNumber = 1;


				while (sr.Peek() >= 0 || currentAStringNumber != currentBStringNumber) {
					a = new char[1];
					b = new char[1];

					sr.Read(a, aIndex++, a.Length);
					sr.Read(b, bIndex++, b.Length);

					// Символы не равны
					if (a != b && sr.Peek() >= 0) {
						// Возвращаем указатель строки А на место
						aIndex = startAString;

						// И переводим строку B на уровень ниже
						MoveToNextString(ref bIndex, sr, ref currentBStringNumber);

						if (sr.Peek() < 0) {
							// Символы не равны, и достигнут конец файла
							// Значит строка А уникальна
							uniqStrResult++;
							//Переходим на следующую строку A
							MoveToNextString(ref aIndex, sr, ref currentAStringNumber);
							startAString = aIndex;
							// Вдруг мы перешли на ту же строку что на которой сейчас указатель B
							if(currentAStringNumber == currentBStringNumber) {
								break;
							}
							// Переводим B указатель в A и ищем конец строки
							bIndex = aIndex;
							currentBStringNumber = currentAStringNumber;
							// И переводим строку B на уровень ниже
							MoveToNextString(ref bIndex, sr, ref currentBStringNumber);
						}

						continue;
					}







					if (a.First() == b.First() && a.First() == '\n') {
						// Сюда попадаем только когда все предыдущие итерации A и B были равны
						currentAStringNumber++;
						startAString = aIndex;
						MoveToNextString(ref bIndex, sr, ref currentBStringNumber);
					}

				}
			}

			
		}

		private static void MoveToNextString(ref int index, StreamReader sr,ref int currentStringNumber) {
			var temp = new char[1];
			do {
				temp = new char[1];
				sr.Read(temp, index++, temp.Length);

				if (temp.First() == '\n') {
					currentStringNumber++;
					break;
				}

			} while (sr.Peek() >= 0);
		}


		/*
* Символы не равны, и sr.Peek() >= 0
*	leftIndex = currentStartLeftIndex;
*	переходим на след строку B
*	currentBStringNumber++;
*	continue;
*
* Символы не равны, и достигнут конец файла sr.Peek() < 0
*	Значит строка А уникальна uniqStrResult++
*		Переходим на следующую строку A currentAStringNumber++
*	    currentStartLeftIndex = leftIndex
*		Проверяем что currentAStringNumber != currentBStringNumber
*		
*		Переводим B указатель в A и ищем конец строки
*			Переходим на след строку currentBStringNumber = currentAStringNumber + 1
*			continue;
*			
* Символы равны и a.First() == '\n'
*	// Сюда попадаем только когда все предыдущие итерации были равны 
*	// и всё закончилось a == b == '\n' - строки одинаковые спускаемся ниже
*		Строка А у нас и так уже перешла ниже, 
*			увеличиваем номер строки currentAStringNumber++
*			currentStartLeftIndex = leftIndex
*		Переводим B указатель в A и ищем конец строки
*			Переходим на след строку currentBStringNumber = currentAStringNumber + 1
*			continue;
* 
*/
	}
}
