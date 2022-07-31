<template>
    <div class="main" :class="{ 'main-hide-text': shrink }">
        <div class="sidebar-menu-con" :style="sidebarMenuStyle">
            <shrinkable-menu
                :shrink="shrink"
                @on-change="handleSubmenuChange"
                :theme="menuTheme"
                :before-push="beforePush"
                :open-names="openedSubmenuArr"
                :menu-list="menuList"
            >
                <div slot="top" class="logo-con">
                    <a>
                        <Icon type="cube" size="32"></Icon>
                        <h1>{{ L("AppName") }}</h1>
                    </a>
                </div>
            </shrinkable-menu>
        </div>
        <div class="main-header-con" :style="mainHeaderStyle">
            <div class="main-header">
                <div class="navicon-con">
                    <Button
                        :style="menuButtonToggleStyle"
                        type="text"
                        @click="toggleClick"
                    >
                        <Icon type="navicon" size="32"></Icon>
                    </Button>
                </div>
                <div class="header-middle-con">
                    <div class="main-breadcrumb">
                        <breadcrumb-nav :currentPath="currentPath" />
                    </div>
                </div>
                <div class="header-avator-con">
                    <full-screen
                        v-model="isFullScreen"
                        @on-change="fullscreenChange"
                    />
                    <lock-screen></lock-screen>
                    <notice></notice>
                    <language-list></language-list>
                    <div class="user-dropdown-menu-con">
                        <Row
                            type="flex"
                            justify="end"
                            align="middle"
                            class="user-dropdown-innercon"
                        >
                            <Dropdown
                                transfer
                                trigger="click"
                                @on-click="handleClickUserDropdown"
                            >
                                <a href="javascript:void(0)">
                                    <span class="main-user-name">
                                        {{ userName }}
                                    </span>
                                    <Icon type="arrow-down-b"></Icon>
                                </a>
                                <DropdownMenu slot="list">
                                    <DropdownItem name="ownSpace">
                                        {{ L("UserProfile") }}
                                    </DropdownItem>
                                    <DropdownItem name="loginout" divided>
                                        {{ L("Logout") }}
                                    </DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                            <span
                                class="avatar"
                                style="background: #619fe7; margin-left: 10px"
                            >
                                <img src="../images/usericon.jpg" />
                            </span>
                        </Row>
                    </div>
                </div>
            </div>
            <div class="tags-con">
                <tags-page-opened :pageTagsList="pageTagsList" />
            </div>
        </div>
        <div class="single-page-con" :style="singlePageStyle">
            <div class="single-page">
                <keep-alive :include="cachePage">
                    <router-view></router-view>
                </keep-alive>
            </div>
            <copyfooter :copyright="L('CopyRight')"></copyfooter>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Watch } from "vue-property-decorator";
import util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import ShrinkableMenu from "@/components/shrinkable-menu/shrinkable-menu.vue";
import TagsPageOpened from "@/components/tags-page-opened.vue";
import BreadcrumbNav from "@/components/breadcrumb-nav.vue";
import FullScreen from "@/components/fullscreen.vue";
import LockScreen from "@/components/lockscreen/lockscreen.vue";
import Notice from "@/components/notices/notice.vue";
import LanguageList from "@/components/language-list.vue";
import Copyfooter from "@/components/Footer.vue";

@Component({
    components: {
        ShrinkableMenu,
        TagsPageOpened,
        BreadcrumbNav,
        FullScreen,
        LockScreen,
        Notice,
        Copyfooter,
        LanguageList,
    },
})
export default class Main extends AbpBase {
    shrink: boolean = false;
    isFullScreen: boolean = false;
    messageCount: string = "0";

    get toggleWidth() {
        return this.shrink ? "80px" : "256px";
    }

    get sidebarMenuStyle() {
        return {
            width: this.toggleWidth,
            overflow: this.shrink ? "visible" : "auto",
        };
    }

    get mainHeaderStyle() {
        return { paddingLeft: this.toggleWidth };
    }

    get menuButtonToggleStyle() {
        return {
            transform: "rotateZ(" + (this.shrink ? "-90" : "0") + "deg)",
        };
    }

    get singlePageStyle() {
        return { left: this.toggleWidth };
    }

    get userName() {
        return this.$store.state.session.user
            ? this.$store.state.session.user.name
            : "";
    }

    get openedSubmenuArr() {
        return this.$store.state.app.openedSubmenuArr;
    }

    get menuList() {
        return this.$store.state.app.menuList;
    }

    get pageTagsList() {
        return this.$store.state.app.pageOpenedList as Array<any>;
    }

    get currentPath() {
        return this.$store.state.app.currentPath;
    }

    get lang() {
        return this.$store.state.app.lang;
    }

    get avatorPath() {
        return localStorage.avatorImgPath;
    }

    get cachePage() {
        return this.$store.state.app.cachePage;
    }

    get menuTheme() {
        return this.$store.state.app.menuTheme;
    }

    get mesCount() {
        return this.$store.state.app.messageCount;
    }

    @Watch("$route")
    routeChange(to: any) {
        this.$store.commit("app/setCurrentPageName", to.name);
        let pathArr = util.setCurrentPath(this, to.name);
        if (pathArr.length > 2) {
            this.$store.commit("app/addOpenSubmenu", pathArr[1].name);
        }
        this.checkTag(to.name);
        localStorage.currentPageName = to.name;
    }

    @Watch("lang")
    langChange() {
        util.setCurrentPath(this, this.$route.name as string);
    }

    created() {
        this.$store.commit("app/setOpenedList");
    }

    mounted() {
        this.init();
    }

    init() {
        let pathArr = util.setCurrentPath(this, this.$route.name as string);
        this.$store.commit("app/updateMenulist");
        if (pathArr.length >= 2) {
            this.$store.commit("app/addOpenSubmenu", pathArr[1].name);
        }
        let messageCount = 3;
        this.messageCount = messageCount.toString();
        this.checkTag(this.$route.name);
    }

    toggleClick() {
        this.shrink = !this.shrink;
    }

    handleClickUserDropdown(name: string) {
        if (name === "ownSpace") {
            util.openNewPage(this, "ownspace_index", null, null);
            this.$router.push({
                name: "ownspace_index",
            });
        } else if (name === "loginout") {
            this.$store.commit("app/logout", this);
            util.abp.auth.clearToken();
            location.reload();
        }
    }

    checkTag(name?: string) {
        let openpageHasTag = this.pageTagsList.some((item: any) => {
            return item.name === name;
        });
        if (!openpageHasTag) {
            util.openNewPage(
                this,
                name as string,
                this.$route.params || {},
                this.$route.query || {}
            );
        }
    }

    handleSubmenuChange(val: string) {
        // console.log(val)
    }

    beforePush(name: string) {
        if (name === "accesstest_index") {
            return false;
        } else {
            return true;
        }
    }

    fullscreenChange(isFullScreen: boolean) {
        // console.log(isFullScreen);
    }
}
</script>

<style lang="less">
@import "./main.less";
</style>
