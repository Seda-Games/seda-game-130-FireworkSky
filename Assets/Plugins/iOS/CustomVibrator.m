//
//  CustomVibrator.m
//  Unity-iPhone
//
//  Created by Donghui Li on 9/19/19.
//

#import <UIKit/UIFeedbackGenerator.h>

void SetVibratorIOS(int intensityLevel){
    UIImpactFeedbackStyle ifs = UIImpactFeedbackStyleLight;
    if(intensityLevel == 1){
        ifs = UIImpactFeedbackStyleMedium;
    } else if(intensityLevel == 2){
        ifs = UIImpactFeedbackStyleHeavy;
    }
    UIImpactFeedbackGenerator *feedBackGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:ifs];
    [feedBackGenerator impactOccurred];
}
