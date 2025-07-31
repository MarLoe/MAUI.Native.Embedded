//
//  MAUI_Native_Embedded_WidgetLiveActivity.swift
//  MAUI.Native.Embedded.Widget
//
//  Created by Martin Løbger on 31/07/2025.
//

import ActivityKit
import WidgetKit
import SwiftUI

struct MAUI_Native_Embedded_WidgetAttributes: ActivityAttributes {
    public struct ContentState: Codable, Hashable {
        // Dynamic stateful properties about your activity go here!
        var emoji: String
    }

    // Fixed non-changing properties about your activity go here!
    var name: String
}

struct MAUI_Native_Embedded_WidgetLiveActivity: Widget {
    var body: some WidgetConfiguration {
        ActivityConfiguration(for: MAUI_Native_Embedded_WidgetAttributes.self) { context in
            // Lock screen/banner UI goes here
            VStack {
                Text("Hello \(context.state.emoji)")
            }
            .activityBackgroundTint(Color.cyan)
            .activitySystemActionForegroundColor(Color.black)

        } dynamicIsland: { context in
            DynamicIsland {
                // Expanded UI goes here.  Compose the expanded UI through
                // various regions, like leading/trailing/center/bottom
                DynamicIslandExpandedRegion(.leading) {
                    Text("Leading")
                }
                DynamicIslandExpandedRegion(.trailing) {
                    Text("Trailing")
                }
                DynamicIslandExpandedRegion(.bottom) {
                    Text("Bottom \(context.state.emoji)")
                    // more content
                }
            } compactLeading: {
                Text("L")
            } compactTrailing: {
                Text("T \(context.state.emoji)")
            } minimal: {
                Text(context.state.emoji)
            }
            .widgetURL(URL(string: "http://www.apple.com"))
            .keylineTint(Color.red)
        }
    }
}

extension MAUI_Native_Embedded_WidgetAttributes {
    fileprivate static var preview: MAUI_Native_Embedded_WidgetAttributes {
        MAUI_Native_Embedded_WidgetAttributes(name: "World")
    }
}

extension MAUI_Native_Embedded_WidgetAttributes.ContentState {
    fileprivate static var smiley: MAUI_Native_Embedded_WidgetAttributes.ContentState {
        MAUI_Native_Embedded_WidgetAttributes.ContentState(emoji: "😀")
     }
     
     fileprivate static var starEyes: MAUI_Native_Embedded_WidgetAttributes.ContentState {
         MAUI_Native_Embedded_WidgetAttributes.ContentState(emoji: "🤩")
     }
}

#Preview("Notification", as: .content, using: MAUI_Native_Embedded_WidgetAttributes.preview) {
   MAUI_Native_Embedded_WidgetLiveActivity()
} contentStates: {
    MAUI_Native_Embedded_WidgetAttributes.ContentState.smiley
    MAUI_Native_Embedded_WidgetAttributes.ContentState.starEyes
}
