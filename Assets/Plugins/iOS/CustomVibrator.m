//
//  CustomVibrator.m
//  Unity-iPhone
//
//  Created by Donhui Li on 9/19/19.
//

#import <UIKit/UIFeedbackGenerator.h>

void setVibratorIOS(){
    UIImpactFeedbackGenerator *feedBackGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
    [feedBackGenerator impactOccurred];
}
