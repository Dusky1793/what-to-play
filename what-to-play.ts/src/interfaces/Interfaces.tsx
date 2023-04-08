interface IAchievement {
    name?: string;
    apiName?: string;
    achieved?: boolean;
    achieved_OldApi?: boolean;
    description?: string;
    iconClosed?: string;
    iconOpen?: string;
    unlocktime?: string;
    unlocktime_DateTime?: Date;
}

interface IAchievementDetails {
    achievementPercentage?: string;
    gameLogo?: string;
    gameLink?: string;
    gameIcon?: string;
    achievements?: IAchievement[];
}

interface IGame {
    appId: string;
    name?: string;
    img_Icon_Url?: string;
    achievementDetails?: IAchievementDetails;
    playtime_Forever_Hours?: string;
    playtime_Forever?: string;
    rtime_Last_Played_DateTime?: Date;
    isSelected?: boolean;
}

interface IGamesState {
    games: IGame[];
}

export type { 
    IAchievement, 
    IAchievementDetails,
    IGame,
    IGamesState
 }