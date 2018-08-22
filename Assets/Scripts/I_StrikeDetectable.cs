using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_StrikeDetectable  {

	bool IsDetectable { get; }
	void StrikeDetect();

}
