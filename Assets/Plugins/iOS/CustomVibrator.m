//
//  CustomVibrator.m
//  Unity-iPhone
//
//  Created by Donghui Li on 9/19/19.
//

#import <UIKit/UIFeedbackGenerator.h>

void SetVibratorIOS(int intensityLevel){
    UIImpactFeedbackStyle ifs = UIImpactFeedbackStyleLight + intensityLevel;
    UIImpactFeedbackGenerator *feedBackGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:ifs];
    [feedBackGenerator impactOccurred];
}
